using MicroService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Service
{
    [NotInject]
    public class TestNotIoc
    {
        public void test() => Console.WriteLine("test not ioc");
    }
}
