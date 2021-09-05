using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core
{
    /// <summary>
    /// mq服务
    /// </summary>
    public static class RabbitMQSetup
    {
        public static void AddRabbitMQSetup(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(ServiceConstants.RabbitMQConfigName);
            services.Configure<RabbitMQConfig>(section);
        }
    }
}
