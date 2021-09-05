using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core
{
    public class EventBusConfig
    {
        public string DbConnection { get; set; }

        public RabbitMQConfig RabbitMQConfig { get; set; }
    }
}
