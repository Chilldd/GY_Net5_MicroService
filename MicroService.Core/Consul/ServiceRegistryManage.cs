using Consul;
using MicroService.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Consul
{
    public class ServiceRegistryManage : IServiceRegistryManage
    {
        private readonly IOptions<ConsulConfig> options;

        public ServiceRegistryManage(IOptions<ConsulConfig> options)
        {
            this.options = options;
        }

        public void Deregister(ConsulConfig config)
        {
            string consulAddress = GetConsulAddress(config);
            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(consulAddress);
            });
            consulClient.Agent.ServiceDeregister(config.ID).Wait();
            consulClient.Dispose();
            ConsoleHelper.WriteSuccessLine($"服务下线成功，ID: 【{config.ID}】, IP: 【{config.Address}】");
        }

        public async Task<List<ServiceInfo>> GetServices(string serviceName)
        {
            var consulClient = new ConsulClient(configuration =>
            {
                configuration.Address = new Uri(GetConsulAddress(options.Value));
            });

            var queryResult = await consulClient.Catalog.Service(serviceName);

            var list = new List<ServiceInfo>();
            foreach (var service in queryResult.Response)
            {
                list.Add(new ServiceInfo
                {
                    ID = service.ServiceID,
                    ServiceName = service.ServiceName,
                    Address = service.ServiceAddress,
                    Port = service.ServicePort
                });
            }
            return list;
        }

        public void Register(ConsulConfig config)
        {
            string consulAddress = GetConsulAddress(config);
            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(consulAddress);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = config.ID,//服务实例唯一标识
                Name = config.ServiceName,//服务名
                Address = config.Address, //服务地址
                Port = config.Port,//服务端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止5s后注销服务
                    Interval = TimeSpan.FromSeconds(config.Interval),//健康检查时间间隔
                    HTTP = $"{config.Address}{config.HealthCheck}",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(config.Timeout)//超时时间
                }
            };

            //服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            consulClient.Dispose();

            ConsoleHelper.WriteSuccessLine($"服务注册成功，ID:【{config.ID}】, IP: 【{config.Address}】");
        }

        private string GetConsulAddress(ConsulConfig config)
        {
            //TODO: https
            return "http://" + config.ConsulIP + ":" + config.ConsulPort;
        }
    }
}
