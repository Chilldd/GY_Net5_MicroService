using MicroService.Common;
using MicroService.SystemManage.Model.Entity;
using Microsoft.AspNetCore.Builder;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.SystemManage
{
    /// <summary>
    /// 全局查询过滤(TODO没生效)
    /// </summary>
    public static class GlobalQueryFilterMildd
    {
        public static void UseGlobalQueryFilterMildd(this IApplicationBuilder app, ISqlSugarClient client)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            //client.QueryFilter.Add(new TableFilterItem<UserEntity>(it => it.del_flag == SqlConstants.NotDelete));
            //client.QueryFilter.Add(new TableFilterItem<RoleEntity>(it => it.del_flag == SqlConstants.NotDelete));

        }
    }
}
