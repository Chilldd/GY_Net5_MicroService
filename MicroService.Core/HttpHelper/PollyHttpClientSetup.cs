using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core
{
    /// <summary>
    /// HttpClient 
    /// </summary>
    public static class PollyHttpClientSetup
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(PollyHttpClientSetup));

        /// <summary>
        /// 添加HttpClient服务，并设置容错机制
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddPollyHttpClient(this IServiceCollection services, string name, IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(ServiceConstants.PollyConfigName);
            services.Configure<PollyHttpClientConfig>(section);

            services.AddHttpClient(name)
                    //异常降级(对熔断器异常降级)
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<BrokenCircuitException>()
                                                                 .FallbackAsync(GetFallBackResponseObject(ResultEnum.Circuit, section.GetValue<string>("httpResponseMessage"))))
                    //异常降级(对超时请求异常降级)
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<TimeoutRejectedException>()
                                                                 .FallbackAsync(GetFallBackResponseObject(ResultEnum.TimeOut, section.GetValue<string>("httpResponseMessage"))))
                    /**
                     * 熔断(对所有异常进行熔断)
                     * 1: 当异常请求数量到达熔断次数时，触发。此时熔断器处于打开状态，后续所有请求直接降级
                     * 2: 熔断器关闭时触发
                     * 3: 请求前，熔断器状态变成半打开，此时如果请求成功，熔断器关闭，请求失败继续打开。
                     */
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>()
                                                                 .CircuitBreakerAsync(section.GetValue<int>("CircuitBreakerOpenFallCount"),
                                                                                      TimeSpan.FromSeconds(section.GetValue<double>("CircuitBreakerDownTime")),
                                                                                      //1
                                                                                      (msg, circuitState, time, context) =>
                                                                                      {
                                                                                          log.Error($"服务异常: 断路器打开, 之前状态: 【{circuitState}】, 时间: 【{time}】, 异常信息: 【{msg.Exception.Message}】", msg.Exception);
                                                                                      },
                                                                                      //2
                                                                                      (context) =>
                                                                                      {
                                                                                          log.Error($"$服务监控: 断路器关闭{JsonConvert.SerializeObject(context)}");
                                                                                      },
                                                                                      //3
                                                                                      () =>
                                                                                      {
                                                                                          log.Info("$服务监控: 熔断器半打开");
                                                                                      }))
                    //超时(请求超时报异常)
                    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(section.GetValue<int>("TimeoutTime"),
                                                                               (context, timeSpan, task, ex) =>
                                                                               {
                                                                                   log.Error($"服务调用超时: 调用时长: 【{timeSpan}】, 异常信息: 【{ex.Message}】", ex);
                                                                                   return Task.CompletedTask;
                                                                               }))
                    //重试
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>()
                                                                 .RetryAsync(section.GetValue<int>("RetryCount"),
                                                                             (res, num, c) =>
                                                                             {
                                                                                 log.Error($"服务重试: 当前次数: 【{num}】, 重试原因: 【{res.Exception.Message}】");
                                                                             }))
                    //资源隔离(参数1: 最大并发线程, 参数2: 等待线程)
                    .AddPolicyHandler(Policy.BulkheadAsync<HttpResponseMessage>(10, 1000));


            return services;
        }


        private static HttpResponseMessage GetFallBackResponseObject(ResultEnum type, string msg) => new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(new ResultEntity<object>(type, msg)))
        };

    }
}
