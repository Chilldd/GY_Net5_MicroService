using Autofac;
using MicroService.Common;
using MicroService.Core;
using MicroService.Core.Authorization;
using MicroService.Core.Consul;
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
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.SystemManage", Version = "v1" });
            });

            services.AddRedisSetup(Configuration);
            services.AddCommonSetup(Configuration);
            services.AddSqlsugarSetup(Configuration);
            services.AddConsulSetup(Configuration);
            services.AddAuthenticationJwtSetup(Configuration);
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

            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGlobalQueryFilterMildd(client);
        }

        /// <summary>
        /// ����Autofac(IOC)
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��������ע��ʵ����AutofacModuleRegister�ͼ̳���Autofac.Module����
            builder.RegisterModule<AutofacModuleRegister>();
        }
    }
}