using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Common
{
    public interface IUserHelper
    {
        UserInfo GetUserInfo();
        string GetUserID();
    }
}
