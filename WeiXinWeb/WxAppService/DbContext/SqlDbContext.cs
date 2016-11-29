using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace WxAppService.DbContext
{
    public class SqlDbContext
    {
        public static SqlSugarClient GetInstance()
        {
            string constr = ConfigurationManager.ConnectionStrings["WxDBConnection"].ConnectionString;
            var db = new SqlSugarClient(constr);
            //db.IsEnableLogEvent = true;//Enable log events
            //db.LogEventStarting = (sql, par) => { Console.WriteLine(sql + " " + par + "\r\n"); };
            return db;
        }
    }
}
