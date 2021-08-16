using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Core
{
    public class ConsulConfig
    {
        /// <summary>
        /// consul id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// consul ip
        /// </summary>
        public string ConsulIP { get; set; }

        /// <summary>
        /// consul 端口
        /// </summary>
        public int ConsulPort { get; set; }

        /// <summary>
        /// 应用地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 应用IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 应用端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheck { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; } = 5;

        /// <summary>
        /// 健康检查时间间隔
        /// </summary>
        public int Interval { get; set; } = 10;
    }
}
