using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool
{
    public class DBType
    {
        public static Dictionary<string, string> GetDBTypes()
        {
            Dictionary<string, string> dir = new Dictionary<string, string>();
            dir["Oracle"] = "Oracle";
            dir["PostgreSQL"] = "PostgreSQL";
            dir["SQLServer"] = "SQLServer";
            dir["MySQL"] = "MySql.Data.MySqlClient";
            dir["Dm"] = "Dm";
            return dir;
        }
    }
}
