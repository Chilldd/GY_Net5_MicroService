using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Consul
{
    public static class ConsulMiddlewares
    {
        /// <summary>
        /// 注册Consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsulMildd(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            IOptions<ConsulConfig> options = app.ApplicationServices.GetService(typeof(IOptions<ConsulConfig>)) as IOptions<ConsulConfig>;
            
            string appAddress = "http://" + options.Value.IP + ":" + options.Value.Port;
            string consulAddress = "http://" + options.Value.ConsulIP + ":" + options.Value.ConsulPort;
            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(consulAddress);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),//服务实例唯一标识
                Name = options.Value.ServiceName,//服务名
                Address = options.Value.IP, //服务IP
                Port = options.Value.Port,//服务端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(options.Value.Interval),//健康检查时间间隔
                    HTTP = $"{appAddress}{options.Value.HealthCheck}",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(options.Value.Timeout)//超时时间
                }
            };

            //服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            //应用程序终止时，取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}
