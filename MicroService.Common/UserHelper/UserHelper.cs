using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Common
{
    /// <summary>
    /// 获取登录人信息
    /// </summary>
    public class UserHelper : IUserHelper
    {
        private readonly HttpContext httpContext;

        public UserHelper(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext.HttpContext;
        }


        public UserInfo GetUserInfo()
        {
            var list = httpContext.User.Claims.Select(e => new { e.Type, e.Value });
            return new UserInfo
            {
                UserID = Convert.ToInt32(list.Where(e => e.Type == ClaimConstants.Claim_UserID).Select(e => e.Value).FirstOrDefault()),
                UserName = list.Where(e => e.Type == ClaimConstants.Claim_UserName).Select(e => e.Value).FirstOrDefault()
            };
        }

        public string getClaimValue(string type)
        {
            return httpContext.User.Claims.Where(e => e.Type == type).Select(e => e.Value).FirstOrDefault();
        }

        public string GetUserID()
        {
            return httpContext.User.Claims.Where(e => e.Type == ClaimConstants.Claim_UserID).Select(e => e.Value).FirstOrDefault();
        }
    }

}
