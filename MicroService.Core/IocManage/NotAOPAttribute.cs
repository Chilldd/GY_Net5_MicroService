using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Core
{
    /// <summary>
    /// 不走切面的类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NotAOPAttribute : Attribute
    {
    }
}
