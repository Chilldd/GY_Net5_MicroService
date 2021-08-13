using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Authorization
{
    /// <summary>
    /// 授权处理程序
    /// </summary>
    public class CustomizeAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        private readonly ILogger<CustomizeAuthorizationHandler> log;
        private readonly HttpContext httpContext;

        public CustomizeAuthorizationHandler(ILogger<CustomizeAuthorizationHandler> log,
                                             IHttpContextAccessor httpContext)
        {
            this.log = log;
            this.httpContext = httpContext.HttpContext;
        }

        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            log.LogInformation("*****************************");
            //TODO: 自己处理业务 
            //context.Fail();

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
