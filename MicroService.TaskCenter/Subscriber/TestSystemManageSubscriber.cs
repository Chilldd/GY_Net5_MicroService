using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.TaskCenter.Subscriber
{
    public class TestSystemManageSubscriber : ICapSubscribe
    {
        [CapSubscribe(name: EventBusConstants.SystemTest)]
        public void CheckReceivedMessage(string msg)
        {
            throw new Exception("1");
            Console.WriteLine(msg);
        }
    }
}
