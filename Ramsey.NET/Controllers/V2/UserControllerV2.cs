using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/user")]
    [ApiController]
    public class UserControllerV2 : ControllerBase
    {
        private readonly IRamseyContext _ramseyContext;

        public UserControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("sync")]
        public async Task<ActionResult> SyncUserAsync([FromBody]UserDtoV2 userDto)
        {
            if (_ramseyContext.RamseyUsers.Any(x => x.UserId == userDto.UserId))
                return StatusCode(200);
            else
                _ramseyContext.RamseyUsers.Add(new Models.RamseyUser
                {
                    UserId = userDto.UserId
                });

            await _ramseyContext.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}