using System;

namespace MicroService
{
    /// <summary>
    /// 事务标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseTranAttribute : Attribute
    {

    }
}
