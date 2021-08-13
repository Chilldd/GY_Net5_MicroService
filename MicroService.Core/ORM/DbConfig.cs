using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.ORM
{
    public class DbConfig
    {
        /// <summary>
        /// 所有数据库, Description和ConId需要一致
        /// </summary>
        public enum DbEnum
        {
            [Description("SystemDB")]
            SystemDb,
            [Description("WorkOrderDB")]
            WorkOrderDb
        }
    }
}
