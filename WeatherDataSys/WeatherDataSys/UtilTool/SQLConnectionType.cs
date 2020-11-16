using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherDataSys.UtilTool
{
    public enum SQLConnectionType
    {
        PostgreSQL = 1,
        Oracle = 2,
        SQLServer = 3
    }
    public class SQLUtils
    {
        public static string pattern = @"\'(\d+)/(\d+)/(\d+) (\d+):(\d+):(\d+)\'";
        public static string prediction_pattern = "@predictiontime";
        public static string correcttime_pattern = "@correcttime";
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

                    MatchCollection predictiontimeMatchs11 = Regex.Matches(sql, prediction_pattern);
                    list = new List<string>();
                    foreach (Match predictiontimeMatch in predictiontimeMatchs11)
                    {
                        if (!list.Contains(predictiontimeMatch.Value))
                        {
                            list.Add(predictiontimeMatch.Value);
                            sql = sql.Replace(predictiontimeMatch.Value, "to_char(\"" + predictiontimeMatch.Value.Replace("@", "") + "\",'hh24')");
                        }
                    }

                    MatchCollection correcttimeMatchs12 = Regex.Matches(sql, correcttime_pattern);
                    list = new List<string>();
                    foreach (Match correcttimeMatch in correcttimeMatchs12)
                    {
                        if (!list.Contains(correcttimeMatch.Value))
                        {
                            list.Add(correcttimeMatch.Value);
                            sql = sql.Replace(correcttimeMatch.Value, "to_char(\"" + correcttimeMatch.Value.Replace("@", "") + "\",'hh24')");
                        }
                    }
                    break;
                case SQLConnectionType.PostgreSQL:
                    //to_char(timestamp, 'HH24:MI:SS') PostgreSQL function
                    MatchCollection predictiontimeMatchs21 = Regex.Matches(sql, prediction_pattern);
                    list = new List<string>();
                    foreach (Match predictiontimeMatch in predictiontimeMatchs21)
                    {
                        if (!list.Contains(predictiontimeMatch.Value))
                        {
                            list.Add(predictiontimeMatch.Value);
                            sql = sql.Replace(predictiontimeMatch.Value, "to_char(\"" + predictiontimeMatch.Value.Replace("@", "") + "\",'hh24')");
                        }
                    }
                    MatchCollection correcttimeMatchs22 = Regex.Matches(sql, correcttime_pattern);
                    list = new List<string>();
                    foreach (Match correcttimeMatch in correcttimeMatchs22)
                    {
                        if (!list.Contains(correcttimeMatch.Value))
                        {
                            list.Add(correcttimeMatch.Value);
                            sql = sql.Replace(correcttimeMatch.Value, "to_char(\"" + correcttimeMatch.Value.Replace("@", "") + "\",'hh24')");
                        }
                    }

                    break;
            }
            return sql;
        }
        public static string genarateSQL(StringBuilder sqlStringBuilder, SQLConnectionType sqlConnectionType)
        {
            /*switch (sqlConnectionType)
            {
                case SQLConnectionType.Oracle:
                    int step = 0;
                    int originalLen = sqlStringBuilder.Length;
                    MatchCollection matchs = Regex.Matches(sqlStringBuilder.ToString(), pattern);
                    for (int i = 0; i < matchs.Count; i++)
                    {
                        if (i == 1)
                        {
                            step = sqlStringBuilder.Length - originalLen;
                        }
                        int indexoffset = step * i;
                        sqlStringBuilder.Replace(matchs[i].Value, "to_date(" + matchs[i].Value + ",'yyyy-mm-dd hh24:mi:ss')", matchs[i].Index + indexoffset, matchs[i].Length);
                    }
                    break;
            }
            return sqlStringBuilder.ToString();*/
            return genarateSQL(sqlStringBuilder.ToString(), sqlConnectionType);
        }
    }

    public class SQLConnectionUtil
    {
        public static char mark = '_';
        public static string defaultName = "Default";
        private List<SQLConnection> _SQLConnections;
        private double _diffDays = 31;   //热点数据库中只存储预报日期从今天起，倒退31天的数据
        public double diffDays
        {
            get { return _diffDays; }
            set { _diffDays = value; }
        }
        public List<SQLConnection> SQLConnections
        {
            get
            {
                if (_SQLConnections == null)
                {
                    _SQLConnections = new List<SQLConnection>();
                    ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
                    foreach (ConnectionStringSettings connection in connections)
                    {
                        if (connection.Name != "LocalSqlServer")
                        {
                            SQLConnection item = new SQLConnection();
                            string[] names = connection.Name.Split(mark);
                            if (names.Length > 1)
                            {
                                item.name = names[1];
                                item.connectionString = connection.ConnectionString;
                                item.providerName = connection.ProviderName;
                                _SQLConnections.Add(item);
                            }
                        }

                    }
                }
                return _SQLConnections;
            }
        }
        public SQLConnection getSQLConnection()
        {
            return SQLConnections.Find(delegate (SQLConnection item) { return item.name == defaultName; });
        }

        /// <summary>
        /// 单个时间点的查询
        /// </summary>
        /// <param name="dateTimePara"></param>
        /// <returns></returns>
        public SQLConnection getSQLConnection(DateTime dateTimePara)
        {
            string paraYear = dateTimePara.Year.ToString();
            //判断DateTime是否在热点库中
            bool isInHotDB = true;
            double paraDayDiff = DateTime.Now.Subtract(dateTimePara).TotalDays;
            if (paraDayDiff >= _diffDays)
            {
                isInHotDB = false;
            }
            if (isInHotDB)
            {
                return getSQLConnection();
            }
            return SQLConnections.Find(delegate (SQLConnection item) { return item.name == paraYear; });
        }

        /// <summary>
        /// 时间段查询
        /// </summary>
        /// <param name="dateTimePara1"></param>
        /// <param name="dateTimePara2"></param>
        /// <returns></returns>
        public List<SQLConnection> getSQLConnection(DateTime startDateTime, DateTime endDateTime)
        {
            List<SQLConnection> results = new List<SQLConnection>();
            double startDateTimeDayDiff = DateTime.Now.Subtract(startDateTime).TotalDays;

            bool isInHotDB = false;
            bool condition1 = startDateTimeDayDiff < _diffDays;

            if (startDateTime < DateTime.Now)
            {
                if (condition1)
                {
                    isInHotDB = true;  //只有一种条件符合在热点库中
                }
                if (isInHotDB)
                {
                    results.Add(getSQLConnection());
                }
                else
                {

                    int yearDiff = endDateTime.Year - startDateTime.Year + 1;

                    for (int i = 0; i < yearDiff; i++)
                    {
                        string currentYear = (startDateTime.Year + i).ToString();

                        SQLConnection sqlConnection = SQLConnections.Find(delegate (SQLConnection item) { return item.name == currentYear; });
                        if (sqlConnection != null)
                        {
                            results.Add(sqlConnection);
                        }
                    }

                }
            }

            return results;
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
