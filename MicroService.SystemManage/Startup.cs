using Autofac;
using MicroService.Common;
using MicroService.Core;
using MicroService.Core.Authorization;
using MicroService.Core.Consul;
using MicroService.Core.ExceptionHandler;
using MicroService.Core.HttpHelper;
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
                        //e.Filters.Add(typeof(GlobalExceptionsFilter));
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        //����ѭ������
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        //��ʹ���շ���ʽ��key
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        //����ʱ���ʽ
                        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                        //���ñ���ʱ�����UTCʱ��
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    });

            services.AddCommonSetup(Configuration);
            services.AddAuthenticationJwtSetup(Configuration);

            services.AddPollyHttpClient("", null);
            services.AddAutoMapperSetup();
            services.AddRedisSetup(Configuration);
            services.AddSqlsugarSetup(Configuration);
            services.AddConsulSetup(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.SystemManage", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);


                // ������ȨС��
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // ��header�����token�����ݵ���̨
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // Jwt Bearer ��֤�������� oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = SystemConstants.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
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

        }

        /// <summary>
        /// ����Autofac(IOC)
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //�������ע��ʵ����AutofacModuleRegister�ͼ̳���Autofac.Module����
            builder.RegisterModule<AutofacModuleRegister>();
        }
    }
}
