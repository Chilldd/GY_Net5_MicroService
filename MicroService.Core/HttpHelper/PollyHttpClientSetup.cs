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

namespace MicroService.Core.HttpHelper
{
    /// <summary>
    /// HttpClient 
    /// </summary>
    public static class PollyHttpClientSetup
    {
        /// <summary>
        /// 添加HttpClient服务，并设置容错机制
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddPollyHttpClient(this IServiceCollection services, string name, Action<PollyHttpClientConfig> action)
        {

            services.AddHttpClient("SystemManage")
                    //异常降级(对熔断器异常降级)
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<BrokenCircuitException>()
                                                                 .FallbackAsync(GetFallBackResponseObject(ResultEnum.Circuit)))
                    //异常降级(对超时请求异常降级)
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<TimeoutRejectedException>()
                                                                 .FallbackAsync(GetFallBackResponseObject(ResultEnum.TimeOut)))
                    //熔断(对所有异常进行熔断)
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>()
                                                                 .CircuitBreakerAsync(3, TimeSpan.FromSeconds(10)))
                    //超时(请求超时报异常)
                    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(30))
                    //重试
                    .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>()
                                                                 .RetryAsync(3,
                                                                             (res, num, c) =>
                                                                             {
                                                                                 Console.WriteLine($"服务重试: 当前次数【{num}】, 重试原因【{res.Exception.Message}】");
                                                                             }))
                    //资源隔离(参数1: 最大并发线程, 参数2: 等待线程)
                    .AddPolicyHandler(Policy.BulkheadAsync<HttpResponseMessage>(10, 1000));


            return services;
        }


        private static HttpResponseMessage GetFallBackResponseObject(ResultEnum type) => new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(new ResultEntity<object>(type)))
        };

    }
}
