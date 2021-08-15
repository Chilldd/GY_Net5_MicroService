using AutoMapper;
using MicroService.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace MicroService.Core
{
    /// <summary>
    /// AutoMapper(实体映射)
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
