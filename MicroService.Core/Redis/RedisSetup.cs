using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Redis
{
    /// <summary>
    /// redis服务
    /// </summary>
    public static class RedisSetup
    {
        public static void AddRedisSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var config = configuration.GetSection(ServiceConstants.RedisConfigName);
            services.Configure<RedisConfig>(config);
            services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();

            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(config.GetValue<string>("ConnectionString"), true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });

        }
    }
}
