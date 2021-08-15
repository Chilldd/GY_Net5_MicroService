using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.ExceptionHandler
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionsFilter> logger;

        public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var model = new ResultEntity<object>();

            model.Msg = context.Exception.Message;
            model.Status = (int)ResultEnum.Error;
            model.Success = false;

            var res = new ContentResult();
            res.Content = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            context.Result = res;

            logger.LogError(res.Content, context.Exception);
        }

    }
}
