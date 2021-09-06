using DotNetCore.CAP;
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
    /// 事件总线
    /// </summary>
    public static class EventBusSetup
    {
        public static IServiceCollection AddCAPEventBusClient(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(ServiceConstants.EventBusConfigName);
            services.Configure<EventBusConfig>(section);

            services.AddCap(e =>
            {
                //启用web ui界面(访问地址: ip:port:/cap)
                e.UseDashboard();
                /**
                 * 启用mysql作为消息数据存储(CAP会自己创建两张表)
                 * cap.published: 消息发送记录表
                 * cap.received: 消息消费记录表
                 */
                e.UseMySql(m =>
                {
                    m.ConnectionString = section.GetValue<string>("DbConnection");
                });
                //启用rabbitmq作为消息中间件
                e.UseRabbitMQ(r =>
                {
                    r.UserName = section.GetValue<string>("MQConfig:UserName");
                    r.Password = section.GetValue<string>("MQConfig:Password");
                    r.VirtualHost = section.GetValue<string>("MQConfig:VirtualHost");
                    r.HostName = section.GetValue<string>("MQConfig:HostName");
                    r.Port = section.GetValue<int>("MQConfig:Port");
                });

                //失败后的重试次数，默认50次；在FailedRetryInterval默认60秒的情况下，即默认重试50*60秒(50分钟)之后放弃失败重试
                //重试规则：前三次立即重试，后续根据设置时间进行重试
                e.FailedRetryCount = section.GetValue<int>("FailedRetryCount");

                //失败后的重试间隔，默认60秒
                e.FailedRetryInterval = section.GetValue<int>("FailedRetryInterval");

                //设置成功信息的删除时间默认24*3600秒    对应表中ExpiresAt字段
                e.SucceedMessageExpiredAfter = section.GetValue<int>("SucceedMessageExpiredAfter");
            });

            //获取当前程序集下所有继承ICapSubscribe接口的类，注入到Ioc容器中
            AppDomain.CurrentDomain
                     .GetAssemblies()
                     .Where(e => e.GetName().Name == AppDomain.CurrentDomain.FriendlyName)
                     .FirstOrDefault()
                     .GetTypes()
                     .Where(e => e.IsAssignableTo(typeof(ICapSubscribe)))
                     .ToList()
                     .ForEach(e =>
                     {
                         services.AddTransient(e);
                     });

            return services;
        }
    }
}
