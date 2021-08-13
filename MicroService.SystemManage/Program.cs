using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                    webBuilder.UseUrls("http://*:10001")
                              .UseStartup<Startup>()
                              .ConfigureLogging((hostingContext, builder) =>
                              {
                                  // 1.���˵�ϵͳĬ�ϵ�һЩ��־
                                  builder.AddFilter("System", LogLevel.Error);
                                  builder.AddFilter("Microsoft", LogLevel.Error);

                                  // Ĭ��log4net.confg
                                  builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
                              });
                });
    }
}