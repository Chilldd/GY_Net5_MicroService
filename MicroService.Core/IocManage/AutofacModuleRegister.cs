using Autofac;
using Autofac.Extras.DynamicProxy;
using log4net;
using MicroService.Common;
using MicroService.Core.AOP;
using MicroService.Core.ORM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.IocManage
{
    /// <summary>
    /// Ioc管理
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            string dllName = Appsettings.app(ServiceConstants.ApplicationName) + ".dll";
            var dllFile = Path.Combine(basePath, dllName);
            var assemblys = Assembly.LoadFrom(dllFile);

            //aop
            var aopList = new List<Type>();
            if (Convert.ToBoolean(Appsettings.app(ServiceConstants.ApplicationAOPLog)))
            {
                builder.RegisterType<FunctionLogAOP>();
                aopList.Add(typeof(FunctionLogAOP));
            }
            if (Convert.ToBoolean(Appsettings.app(ServiceConstants.ApplicationTran)))
            {
                builder.RegisterType<TranAOP>();
                aopList.Add(typeof(TranAOP));
            }

            //注册走aop的类
            builder.RegisterAssemblyTypes(assemblys)
                   .PublicOnly()
                   .Where(e =>
                   {
                       var inject = e.GetCustomAttribute<NotInjectAttribute>();
                       if (inject is not null)
                           return false;
                       var aop = e.GetCustomAttribute<NotAOPAttribute>();

                       if (aop is not null)
                           return false;

                       return e.IsClass;
                   })
                   .InstancePerDependency()
                   .EnableClassInterceptors()   //开启拦截, 只会拦截虚方法，重写方法
                   .InterceptedBy(aopList.ToArray());

            //注册不走aop的类
            builder.RegisterAssemblyTypes(assemblys)  //获取命名空间
                   .PublicOnly()   //只要public访问权限的
                   .Where(e =>
                   {
                       var attr = e.GetCustomAttribute<NotAOPAttribute>();
                       return attr is not null;
                   })
                   .InstancePerDependency();

            //注册仓储
            builder.RegisterType<UnitOfWork>().InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseRepository<>)).InstancePerDependency();
        }
    }
}
