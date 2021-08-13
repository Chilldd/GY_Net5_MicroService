using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Model.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("tb_user_info")]
    public class UserEntity : BaseEntity
    {
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "account")]
        public string Account { get; set; }

        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }

        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }
    }
}
