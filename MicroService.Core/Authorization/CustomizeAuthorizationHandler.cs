using Microsoft.AspNetCore.Authentication;
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
        private readonly IAuthenticationSchemeProvider schemes;
        private readonly ILogger<CustomizeAuthorizationHandler> log;
        private readonly HttpContext httpContext;

        public CustomizeAuthorizationHandler(IAuthenticationSchemeProvider schemes,
                                             ILogger<CustomizeAuthorizationHandler> log,
                                             IHttpContextAccessor httpContext)
        {
            this.schemes = schemes;
            this.log = log;
            this.httpContext = httpContext.HttpContext;
        }

        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            try
            {
                //是否经过认证
                var isAuthenticated = context.User.Identity.IsAuthenticated;
                if (isAuthenticated)
                {
                    //TODO: 自己实现授权逻辑
                    context.Succeed(requirement);
                }
                else
                    context.Fail();
            }
            catch (Exception ex)
            {
                context.Fail();
            }
        }
    }
}
