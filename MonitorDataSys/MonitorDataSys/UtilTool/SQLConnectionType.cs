using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool
{
    public enum SQLConnectionType
    {
        PostgreSQL = 1,
        Oracle = 2,
        SQLServer = 3,
        MySQL = 4,
        Dm = 5
    }
    public class SQLUtils
    {
        public static string pattern = @"\'(\d+)/(\d+)/(\d+) (\d+):(\d+):(\d+)\'";
        public static string monitorTime_pattern = "MONITORTIME";
        public static string genarateSQL(string sql, SQLConnectionType sqlConnectionType)
        {
            switch (sqlConnectionType)
            {
                case SQLConnectionType.Oracle:
                    MatchCollection dateMatchs = Regex.Matches(sql, pattern);
                    List<string> list = new List<string>();
                    foreach (Match dateMatch in dateMatchs)
                    {
                        if (!list.Contains(dateMatch.Value))//去除重复的匹配项
                        {
                            list.Add(dateMatch.Value);
                            sql = sql.Replace(dateMatch.Value, "to_date(" + dateMatch.Value + ",'yyyy-mm-dd hh24:mi:ss')");
                        }
                    }
                    MatchCollection predictiontimeMatchs11 = Regex.Matches(sql, monitorTime_pattern);
                    list = new List<string>();
                    foreach (Match predictiontimeMatch in predictiontimeMatchs11)
                    {
                        if (!list.Contains(predictiontimeMatch.Value))
                        {
                            list.Add(predictiontimeMatch.Value);
                            sql = sql.Replace(predictiontimeMatch.Value, "to_char(\"" + predictiontimeMatch.Value.Replace("@", "") + "\",'hh24')");
                        }
                    }
                    break;
                case SQLConnectionType.PostgreSQL:
                    MatchCollection predictiontimeMatchs21 = Regex.Matches(sql, monitorTime_pattern);
                    list = new List<string>();
                    foreach (Match predictiontimeMatch in predictiontimeMatchs21)
                    {
                        if (!list.Contains(predictiontimeMatch.Value))
                        {
                            list.Add(predictiontimeMatch.Value);
                            sql = sql.Replace(predictiontimeMatch.Value, "to_char(\"" + predictiontimeMatch.Value.Replace("@", "") + "\",'hh24')");
                        }
                    }
                    break;
            }
            return sql;
        }
    }


    public class SQLConnection
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _connectionString;
        public string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        private string _providerName;
        public string providerName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }
    }
}
