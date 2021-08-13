using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Model.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [SugarTable("tb_role_info")]
    public class RoleEntity : BaseEntity
    {
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "desc")]
        public string Desc { get; set; }
    }
}
