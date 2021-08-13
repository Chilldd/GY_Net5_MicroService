using MicroService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Service
{
    public class TestIoc 
    {
        public virtual async Task test()
        {
            await Task.Delay(3000);
            Console.WriteLine("test ioc");
        }
    }
}
