using MicroService.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Authorization
{
    /// <summary>
    /// JWT认证服务
    /// </summary>
    public static class AuthenticationJwtSetup
    {
        public static void AddAuthenticationJwtSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            IConfiguration config = configuration.GetSection(ServiceConstants.AuthenticationConfigName);
            services.Configure<AuthenticationConfig>(config);

            var symmetricKeyAsBase64 = config["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = config["Issuer"];
            var Audience = config["Audience"];

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(config.GetValue<int>("Timeout")),
                RequireExpirationTime = true,
            };

            services.AddAuthorization(o =>
                    {
                    })
                    .AddAuthentication(o =>
                    {
                        // 开启Bearer认证
                        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        //身份认证处理程序
                        o.DefaultChallengeScheme = nameof(CustomizeAuthenticationHandler);
                        o.DefaultForbidScheme = nameof(CustomizeAuthenticationHandler);
                    })
                    .AddJwtBearer(o =>
                    {
                        // 添加JwtBearer服务
                        o.TokenValidationParameters = tokenValidationParameters;
                    })
                    .AddScheme<AuthenticationSchemeOptions, CustomizeAuthenticationHandler>(nameof(CustomizeAuthenticationHandler), o => { });

            services.AddScoped<IAuthorizationHandler, CustomizeAuthorizationHandler>();
        }
    }
}
