using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService
{
    public static class SystemConstants
    {
        public static readonly string Bearer = "Bearer";
        public static readonly string Authorization = "Authorization";
        public static string PolicyName = "Permission";
    }

    public static class ServiceConstants
    {
        public static readonly string DB = "DB";
        public static readonly string ConsulConfigName = "Server:Consul";
        public static readonly string RedisConfigName = "Server:Redis";
        public static readonly string AuthenticationConfigName = "Application:Audience";
        public static readonly string ApplicationName = "Application:ApplicationName";

        public static readonly string ApplicationAOPLog = "Application:AOP:Log";
        public static readonly string ApplicationSqlLog = "Application:AOP:SqlLog";
        public static readonly string ApplicationTran = "Application:AOP:Tran";
    }

    public static class SqlConstants
    {
        public static readonly int Delete = 0;
        public static readonly int NotDelete = 1;
    }

    public class ClaimConstants
    {
        public static readonly string Claim_UserID = "UserID";
        public static readonly string Claim_UserName = "UserName";
    }
}
