using Autofac;
using MicroService.Common;
using MicroService.Core;
using MicroService.Core.Authorization;
using MicroService.Core.Consul;
using MicroService.Core.ExceptionHandler;
using MicroService.Core.IocManage;
using MicroService.Core.ORM;
using MicroService.Core.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MicroService.SystemManage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IServiceCollection _services;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            services.AddControllers(e =>
                    {
                        e.Filters.Add(typeof(GlobalExceptionsFilter));
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        //忽略循环引用
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        //不使用驼峰样式的key
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        //设置时间格式
                        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                        //设置本地时间而非UTC时间
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    });

            services.AddCommonSetup(Configuration);
            services.AddAuthenticationJwtSetup(Configuration);

            services.AddPollyHttpClient(Configuration[ServiceConstants.ApplicationName], Configuration);
            services.AddAutoMapperSetup();
            services.AddRedisSetup(Configuration);
            services.AddSqlsugarSetup(Configuration);
            services.AddConsulSetup(Configuration);
            services.AddSwaggerSetup();
            services.AddCAPEventBusClient(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, ISqlSugarClient client)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService.SystemManage v1"));
            }

            app.UseAllServiceMilddleware(_services);
            app.UseConsulMildd(lifetime);

            app.UseRouting();

            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        /// <summary>
        /// 集成Autofac(IOC)
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //添加依赖注入实例，AutofacModuleRegister就继承自Autofac.Module的类
            builder.RegisterModule<AutofacModuleRegister>();
        }
    }
}
