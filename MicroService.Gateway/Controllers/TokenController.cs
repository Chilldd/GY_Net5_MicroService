using MicroService.Common;
using MicroService.Core.Authorization;
using MicroService.Core.JwtHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Gateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly AuthenticationConfig options;
        private readonly IUserHelper userHelper;
        private readonly ILogger<TokenController> log;

        public TokenController(IOptions<AuthenticationConfig> options, 
                               IUserHelper userHelper,
                               ILogger<TokenController> log)
        {
            this.options = options?.Value;
            this.userHelper = userHelper;
            this.log = log;
        }

        [HttpPost("getToken")]
        public string GetToken()
        {
            var user = new UserInfo
            {
                UserID = 1,
                UserName = "admin"
            };
            return JwtHelper.IssueJwt(options, user);
        }

        [Authorize]
        [HttpGet("testAuth")]
        public UserInfo TestAuth()
        {
            log.LogInformation("test");
            return userHelper.GetUserInfo();
        }
    }
}
