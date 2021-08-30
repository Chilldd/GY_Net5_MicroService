using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Winton.Extensions.Configuration.Consul;

namespace MicroService.SystemManage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .ConfigureLogging((hostingContext, builder) =>
                              {
                                  // 1.过滤掉系统默认的一些日志
                                  builder.AddFilter("System", LogLevel.Error);
                                  builder.AddFilter("Microsoft", LogLevel.Error);

                                  // 默认log4net.confg
                                  builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
                              })
                              .ConfigureAppConfiguration((hostingContext, config) =>
                              {
                                  var env = hostingContext.HostingEnvironment;
                                  hostingContext.Configuration = config.Build();
                                  string consulAddress = "http://192.168.1.130:8501";
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
                              });
                });
    }
}
