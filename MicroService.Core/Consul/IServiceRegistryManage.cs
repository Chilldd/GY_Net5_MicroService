using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.Core.Consul
{
    /// <summary>
    /// 服务管理
    /// </summary>
    public interface IServiceRegistryManage
    {
        /// <summary>
        /// 注册
        /// </summary>
        void Register(ConsulConfig config);

        /// <summary>
        /// 下线
        /// </summary>
        void Deregister(ConsulConfig config);

        /// <summary>
        /// 获取服务
        /// </summary>
        Task<List<ServiceInfo>> GetServices(string serviceName);
    }
}
