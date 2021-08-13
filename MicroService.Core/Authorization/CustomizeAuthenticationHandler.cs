using MicroService.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MicroService.Core.Authorization
{
    /// <summary>
    /// 认证处理程序
    /// </summary>
    public class CustomizeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomizeAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                                        ILoggerFactory logger, 
                                        UrlEncoder encoder, 
                                        ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 认证失败时触发
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ResultEntity<object>(ResultEnum.Unauthorized)));
        }

        /// <summary>
        /// 授权失败时触发
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync(JsonConvert.SerializeObject(new ResultEntity<object>(ResultEnum.Forbidden)));
        }
    }
}
