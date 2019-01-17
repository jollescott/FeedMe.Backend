using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ramsey.NET.Dtos;
using Ramsey.NET.Helpers;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace Ramsey.NET.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private IAdminService _adminService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AdminController(
            IAdminService adminService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _adminService = adminService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AdminDto userDto)
        {
            var user = _adminService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async System.Threading.Tasks.Task<IActionResult> RegisterAsync([FromBody]AdminDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<AdminUser>(userDto);

            try
            {
                // save 
                await _adminService.CreateAsync(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("reindex")]
        public IActionResult Reindex()
        {
            BackgroundJob.Enqueue<ICrawlerService>(x => x.UpdateIndexAsync());
            return StatusCode(200);
        }

        [HttpPost]
        [Route("patch")]
        public IActionResult Patch()
        {
            BackgroundJob.Enqueue<IPatcherService>(x => x.PatchIngredientsAsync());
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _adminService.GetAll();
            var userDtos = _mapper.Map<IList<AdminDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _adminService.GetById(id);
            var userDto = _mapper.Map<AdminDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]AdminDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<AdminUser>(userDto);
            user.Id = id;

            try
            {
                // save 
                _adminService.UpdateAsync(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _adminService.DeleteAsync(id);
            return Ok();
        }
    }
}

