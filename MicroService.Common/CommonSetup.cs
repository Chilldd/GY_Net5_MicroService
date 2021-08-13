using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Common
{
    public static class CommonSetup
    {
        public static void AddCommonSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IUserHelper, UserHelper>();
            services.AddSingleton(new Appsettings(Configuration));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
