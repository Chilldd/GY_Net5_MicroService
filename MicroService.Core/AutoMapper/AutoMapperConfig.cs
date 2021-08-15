using AutoMapper;
using MicroService.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.AutoMapper
{
    /// <summary>
    /// AutoMapper 配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            var basePath = AppContext.BaseDirectory;
            string dllName = Appsettings.app(ServiceConstants.ApplicationName) + ".dll";
            var dllFile = Path.Combine(basePath, dllName);
            var assemblys = Assembly.LoadFrom(dllFile);
            return new MapperConfiguration(cfg => cfg.AddMaps(assemblys));
        }
    }
}
