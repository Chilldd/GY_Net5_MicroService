using MicroService.Core.ORM;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core
{
    public class BaseRepository<T> : SimpleClient<T> where T : class, new()
    {
        public BaseRepository(UnitOfWork context) : base(context.GetDbClient())//注意这里要有默认值等于null
        {
            
        }
    }
}
