using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winton.Extensions.Configuration.Consul;

namespace MicroService.Core
{
    public static class ConfigCenterSetup
    {
        public static void AddAppsettingsJson(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            //获取当前主机环境
            var env = hostingContext.HostingEnvironment;
            hostingContext.Configuration = config.Build();
            //获取配置中心地址
            string consulAddress = "http://192.168.1.130:8501";
            //获取配置文件名称
            string configName = $"{env.ApplicationName}/appsettings.{env.EnvironmentName}.json";
            config.AddConsul(configName,
                             options =>
                             {
                                 options.Optional = true;
                                 options.ReloadOnChange = true;
                                 options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                                 options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consulAddress); };
                             });

            hostingContext.Configuration = config.Build();
        }
    }
}
