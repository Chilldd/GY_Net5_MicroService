using Castle.DynamicProxy;
using MicroService.Common;
using MicroService.Core.ORM;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.AOP
{
    /// <summary>
    /// 事务切面
    /// </summary>
    public class TranAOP : IInterceptor
    {
        private readonly UnitOfWork unitOfWork;
        private readonly ILogger<TranAOP> logger;

        public TranAOP(UnitOfWork unitOfWork, ILogger<TranAOP> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(UseTranAttribute)) is UseTranAttribute)
            {
                try
                {
                    logger.LogInformation($"【{unitOfWork.GetDbClient().ContextID}】事务开始");
                    ConsoleHelper.WriteInfoLine($"【{unitOfWork.GetDbClient().ContextID}】事务开始", ConsoleColor.Green);

                    unitOfWork.BeginTran();

                    invocation.Proceed();

                    if (AOPHelper.IsAsyncMethod(invocation.Method))
                    {
                        var result = invocation.ReturnValue;
                        if (result is Task)
                        {
                            Task.WaitAll(result as Task);
                        }
                    }
                    unitOfWork.CommitTran();
                    logger.LogInformation($"【{unitOfWork.GetDbClient().ContextID}】事务已提交");
                    ConsoleHelper.WriteInfoLine($"【{unitOfWork.GetDbClient().ContextID}】事务已提交", ConsoleColor.Green);
                }
                catch (Exception)
                {
                    logger.LogInformation($"【{unitOfWork.GetDbClient().ContextID}】事务已回滚");
                    ConsoleHelper.WriteInfoLine($"【{unitOfWork.GetDbClient().ContextID}】事务已回滚", ConsoleColor.Red);
                    unitOfWork.RollbackTran();
                }
            }
            else
            {
                invocation.Proceed();
            }

        }

    }
}
