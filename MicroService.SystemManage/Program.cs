using Autofac.Extensions.DependencyInjection;
using MicroService.Core;
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

namespace MicroService.SystemManage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("测试");
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
                                  //添加appsettings配置文件
                                  ConfigCenterSetup.AddAppsettingsJson(hostingContext, config);
                              });
                });
    }
}
