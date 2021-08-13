using Castle.DynamicProxy;
using MicroService.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MicroService.Core.AOP
{
    /// <summary>
    /// 拦截器BlogLogAOP 继承IInterceptor接口
    /// </summary>
    public class FunctionLogAOP : IInterceptor
    {
        private readonly IUserHelper user;
        private readonly ILogger<FunctionLogAOP> log;

        public FunctionLogAOP(IUserHelper user, ILogger<FunctionLogAOP> log)
        {
            this.user = user;
            this.log = log;
        }


        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息
            var dataIntercept = $"【当前操作用户】：{ user?.GetUserID()} \r\n" +
                                $"【当前执行方法】：{ invocation.Method.Name} \r\n" +
                                $"【携带的参数有】： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
            invocation.Proceed();
            
            //判断是不是异步方法
            if (AOPHelper.IsAsyncMethod(invocation.Method))
            {
                //获取返回值
                var result = invocation.ReturnValue;
                if (result is Task)
                {
                    //等待方法执行完成
                    Task.WaitAll(result as Task);
                }
            }
            stopwatch.Stop();
            dataIntercept += ($"【执行时间】：{stopwatch.Elapsed}");
            log.LogInformation($"【AOPLog】\r\n{dataIntercept}");
        }

    }
}
