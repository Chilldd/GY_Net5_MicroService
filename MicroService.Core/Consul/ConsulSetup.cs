using MicroService.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Consul
{
    public static class ConsulSetup
    {
        public static void AddConsulSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Configure<ConsulConfig>(configuration.GetSection(ServiceConstants.ConsulConfigName));
        }
    }
}
