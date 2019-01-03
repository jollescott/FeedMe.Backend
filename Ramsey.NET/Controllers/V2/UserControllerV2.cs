using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers.V2
{
    [Route("api/v2/users")]
    [ApiController]
    public class UserControllerV2 : ControllerBase
    {
        private readonly IRamseyContext _ramseyContext;

        public UserControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("sync")]
        public async Task<ActionResult> SyncUserAsync([FromBody]UserDto userDto)
        {
            if (_ramseyContext.Users.Any(x => x.UserId == userDto.UserId))
                return StatusCode(200);
            else
                _ramseyContext.Users.Add(new Models.User
                {
                    UserId = userDto.UserId
                });

            await _ramseyContext.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}