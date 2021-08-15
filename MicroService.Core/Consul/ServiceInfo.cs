using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.Consul
{
    public class ServiceInfo
    {
        public string ID { get; set; }

        public string ServiceName { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }
    }
}
