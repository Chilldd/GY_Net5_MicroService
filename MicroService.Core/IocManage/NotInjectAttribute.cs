using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Core
{
    /// <summary>
    /// 不在ioc中注册的类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NotInjectAttribute : Attribute
    {
    }
}
