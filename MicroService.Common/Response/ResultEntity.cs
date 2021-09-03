using MicroService.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroService
{
    public class ResultEntity<T>
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }

        public ResultEntity()
        {
            this.Status = (int)ResultEnum.Success;
            this.Msg = ResultEnum.Success.GetEnumDescription();
            this.Success = true;
        }

        public ResultEntity(ResultEnum @enum)
        {
            this.Status = (int)@enum;
            this.Msg = @enum.GetEnumDescription();
            this.Success = @enum == ResultEnum.Success;
        }

        public ResultEntity(ResultEnum @enum, string msg)
        {
            this.Status = (int)@enum;
            this.Msg = msg;
            this.Success = @enum == ResultEnum.Success;
        }

        public ResultEntity(ResultEnum @enum, T data)
        {
            this.Status = (int)@enum;
            this.Msg = @enum.GetEnumDescription();
            this.Success = @enum == ResultEnum.Success;
            this.Data = data;
        }
    }

    public enum ResultEnum
    {
        [Description("成功")]
        Success = 200,
        [Description("认证失败")]
        Unauthorized = 401,
        [Description("授权失败")]
        Forbidden = 403,
        [Description("超时")]
        TimeOut = 408,
        [Description("后台系统报错")]
        Error = 500,
        [Description("服务熔断")]
        Circuit = 504
    }
}
