using log4net;
using MicroService.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Core.ORM
{
    /// <summary>
    /// sqlsugar服务
    /// </summary>
    public static class SqlsugarSetup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlsugarSetup));

        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var dbs = configuration.GetSection(ServiceConstants.DB);
            if (!dbs.Exists())
            {
                throw new Exception("数据库连接未配置");
            }
            var dbCons = dbs.GetChildren();

            services.AddScoped<ISqlSugarClient>(e =>
            {
                //数据库连接
                var configs = new List<ConnectionConfig>();
                foreach (var item in dbCons)
                {
                    if (Convert.ToBoolean(item["Enabled"]))
                        configs.Add(new ConnectionConfig
                        {
                            ConfigId = item["ConId"],
                            ConnectionString = item["Connection"],
                            DbType = (DbType)Enum.Parse(typeof(DbType), item["DBType"]),
                            IsAutoCloseConnection = true,
                            AopEvents = new AopEvents
                            {
                                OnLogExecuting = (sql, p) =>
                                {
                                    if (configuration.GetValue<bool>(ServiceConstants.ApplicationSqlLog))
                                    {
                                        log.Info($"SQL: {GetParas(p)}. 【SQL语句】: {sql}");
                                        ConsoleHelper.WriteColorLine(string.Join("\r\n", new string[] { "-----------------", "【SQL语句】：", GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                    }
                                }
                            }
                        });
                }

                return new SqlSugarClient(configs);
            });
        }

        private static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql.Replace(param.ParameterName, param.Value?.ToString());
            }

            return sql;
        }

        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            if (pars?.Length > 0)
            {
                foreach (var param in pars)
                {
                    key += $"{param.ParameterName}:{param.Value}\n";
                }
            }
            else
            {
                key += "参数为空";
            }

            return key;
        }
    }
}
