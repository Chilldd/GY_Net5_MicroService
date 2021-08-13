using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService
{
    public class BaseEntity
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true, ColumnName = "id")]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "create_user")]
        public int CreateUser { get; set; }

        [SugarColumn(ColumnName = "update_time")]
        public DateTime UpdateTime { get; set; }

        [SugarColumn(ColumnName = "update_user")]
        public int UPdateUser { get; set; }

        [SugarColumn(ColumnName = "del_flag")]
        public int DelFlag { get; set; }
    }
}
