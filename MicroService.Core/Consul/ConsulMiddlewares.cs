using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
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
            options.Value.ID = Guid.NewGuid().ToString();

            //获取服务地址
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            options.Value.Address = address;
            var uri = new Uri(address);
            options.Value.Port = uri.Port;

            IServiceRegistryManage service = app.ApplicationServices.GetService(typeof(IServiceRegistryManage)) as IServiceRegistryManage;

            //注册
            service.Register(options.Value);

            //应用程序终止时，取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                service.Deregister(options.Value);
            });
            return app;
        }
    }
}
