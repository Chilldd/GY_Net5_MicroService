using MicroService.Common;
using MicroService.Core.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.JwtHelper
{
    /// <summary>
    /// jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="config">jwt配置</param>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public static string IssueJwt(AuthenticationConfig config, UserInfo userInfo)
        {
            var claims = new List<Claim>
                {
                new Claim(ClaimConstants.Claim_UserID, userInfo.UserID.ToString()),
                new Claim(ClaimConstants.Claim_UserName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(config.Timeout)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(config.Timeout).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,config.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,config.Audience),
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Audience,
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddMinutes(config.Timeout));

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

    }
}
