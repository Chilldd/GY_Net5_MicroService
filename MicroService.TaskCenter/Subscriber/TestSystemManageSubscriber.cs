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
            Console.WriteLine(msg);
        }
    }
}
