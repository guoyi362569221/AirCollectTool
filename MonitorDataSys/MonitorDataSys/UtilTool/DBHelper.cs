#region DIRECTIVES

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using log4net;

#endregion

namespace MonitorDataSys.UtilTool
{
    class DBHelper
    {

        #region DECLARATIONS

        private DbProviderFactory oFactory;
        private DbConnection oConnection;
        private ConnectionState oConnectionState;
        public DbCommand oCommand;
        private DbParameter oParameter;
        private DbTransaction oTransaction;
        private bool mblTransaction;

        //private static readonly string S_CONNECTION     = ConfigurationManager.AppSettings["DATA.CONNECTIONSTRING"];
        //private static readonly string S_PROVIDER       = ConfigurationManager.AppSettings["DATA.PROVIDER"];

        public string S_CONNECTION;
        public string S_PROVIDER;

        #endregion

        #region ENUMERATORS

        public enum TransactionType : uint
        {
            Open = 1,
            Commit = 2,
            Rollback = 3
        }

        #endregion

        #region STRUCTURES

        public struct Parameters
        {
            public string ParamName;
            public object ParamValue;
            public ParameterDirection ParamDirection;
            public DbType ParamType;

            public Parameters(string Name, object Value, DbType type, ParameterDirection Direction)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamType = type;
                ParamDirection = Direction;
            }

            public Parameters(string Name, object Value, DbType type)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamType = type;
                ParamDirection = ParameterDirection.Input;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                if (obj.GetType() == typeof(Parameters))
                {
                    return this.ParamName.Equals(((Parameters)obj).ParamName, StringComparison.InvariantCultureIgnoreCase);
                }

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return ParamName.GetHashCode();
            }
        }

        #endregion

        #region CONSTRUCTOR

        public SQLConnectionType sqlConnectionType { set; get; }
        public DBHelper()
        {
            //ConnectionStringSettings conectionStringSetting = GetConnection();
            //S_CONNECTION = conectionStringSetting.ConnectionString;
            //S_PROVIDER = conectionStringSetting.ProviderName;
            //setConnectionType(S_PROVIDER);
            //oFactory = DbProviderFactories.GetFactory(S_PROVIDER);
            //mblTransaction = false;
            //Loghelper.WriteLog("连接参数: " + S_CONNECTION);
        }

        public DBHelper(ConnectionStringSettings conectionStringSetting)
        {
            S_CONNECTION = conectionStringSetting.ConnectionString;
            S_PROVIDER = conectionStringSetting.ProviderName;
            setConnectionType(S_PROVIDER);
            oFactory = DbProviderFactories.GetFactory(S_PROVIDER);
            mblTransaction = false;
            Loghelper.WriteLog("连接参数: " + S_CONNECTION);
        }

        public DBHelper(SQLConnection conectionStringSetting)
        {
            S_CONNECTION = conectionStringSetting.connectionString;
            S_PROVIDER = conectionStringSetting.providerName;
            setConnectionType(S_PROVIDER);
            oFactory = DbProviderFactories.GetFactory(S_PROVIDER);
            mblTransaction = false;
        }

        private void setConnectionType(string providerName)
        {
            switch (providerName)
            {
                case "Oracle":
                    sqlConnectionType = SQLConnectionType.Oracle;
                    break;
                case "PostgreSQL":
                    sqlConnectionType = SQLConnectionType.PostgreSQL;
                    break;
                case "SQLServer":
                    sqlConnectionType = SQLConnectionType.SQLServer;
                    break;
                case "MySQL":
                case "MySql.Data.MySqlClient":
                case "MySql.Data":
                    sqlConnectionType = SQLConnectionType.MySQL;
                    break;
                case "Dm":
                    sqlConnectionType = SQLConnectionType.Dm;
                    break;
            }
        }

        /// <summary>
        /// 自定义连接数据库字符串函数
        /// </summary>
        /// <param name="dbServerIP">数据库服务器对应IP</param>
        /// <param name="dbServerPort">数据库服务器对应端口</param>
        /// <param name="dbServerUserId">数据库用户名</param>
        /// <param name="dbServerUserPassword">数据库密码</param>
        /// <param name="providerName">对应数据库类型，PostgreSQL，Oracle，SQLServer</param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public ConnectionStringSettings GetSQLConnection(string dbServerIP, string dbServerPort, string dbServerUserId, string dbServerUserPassword, string providerName, string dbName)
        {
            ConnectionStringSettings con = new ConnectionStringSettings();
            con.ProviderName = providerName;
            switch (con.ProviderName)
            {
                case "PostgreSQL":
                    con.ConnectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Pooling=true;Protocol=3;MinPoolSize=3; MaxPoolSize=20; Encoding=UNICODE; CommandTimeout = 3600; SslMode=Disable", dbServerIP, dbServerPort, dbServerUserId, dbServerUserPassword, dbName);
                    break;
                case "Oracle":
                    con.ConnectionString = String.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User Id={2};Password={3}", dbServerIP, dbServerPort, dbServerUserId, dbServerUserPassword);
                    break;
                case "SQLServer":
                    string portFilter = "";
                    if (!String.IsNullOrEmpty(dbServerPort))
                    {
                        portFilter = "," + dbServerPort;
                    }
                    con.ConnectionString = String.Format("Data Source={0}{1};User ID={2};Password={3};Initial Catalog={4};Persist Security Info=False;Pooling=true;Connection Lifetime=15;Connect Timeout=120;MAX Pool Size=100;Min Pool Size=1", dbServerIP, portFilter, dbServerUserId, dbServerUserPassword, dbName);
                    break;
                case "MySQL":
                case "MySql.Data.MySqlClient":
                case "MySql.Data":
                    con.ConnectionString = String.Format("server = {0}; port = {1}; user = {2}; password = {3}; database = {4}; CharSet = utf8", dbServerIP, dbServerPort, dbServerUserId, dbServerUserPassword, dbName);
                    break;
                case "Dm":
                    sqlConnectionType = SQLConnectionType.Dm;
                    con.ConnectionString = String.Format("server = {0}; port = {1}; user = {2}; password = {3}", dbServerIP, dbServerPort, dbServerUserId, dbServerUserPassword, dbName);
                    break;
            }
            return con;
        }
        #endregion

        #region DESTRUCTOR

        ~DBHelper()
        {
            oFactory = null;
        }

        #endregion

        #region CONNECTIONS

        public void EstablishFactoryConnection()
        {
            /*
            // This check is not required as it will throw "Invalid Provider Exception" on the contructor itself.
            if (0 == DbProviderFactories.GetFactoryClasses().Select("InvariantName='" + S_PROVIDER + "'").Length)
                throw new Exception("Invalid Provider");
            */
            try
            {
                oConnection = oFactory.CreateConnection();

                if (oConnection.State == ConnectionState.Closed)
                {
                    oConnection.ConnectionString = S_CONNECTION;
                    oConnection.Open();
                    oConnectionState = ConnectionState.Open;
                }
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("数据库连接错误", e);
                throw new Exception("数据库连接错误");
            }
        }


        public void CloseFactoryConnection()
        {
            //check for an open connection            
            try
            {
                if (oConnection.State == ConnectionState.Open)
                {
                    oConnection.Close();
                    oConnectionState = ConnectionState.Closed;
                }
            }
            catch (DbException oDbErr)
            {
                //catch any SQL server data provider generated error messag
                throw new Exception(oDbErr.Message);
            }
            catch (System.NullReferenceException oNullErr)
            {
                throw new Exception(oNullErr.Message);
            }
            finally
            {
                if (null != oConnection)
                    oConnection.Dispose();
            }
        }

        #endregion

        #region TRANSACTION


        public void TransactionHandler(TransactionType veTransactionType)
        {
            switch (veTransactionType)
            {
                case TransactionType.Open:  //open a transaction
                    try
                    {
                        EstablishFactoryConnection();
                        oTransaction = oConnection.BeginTransaction();
                        mblTransaction = true;
                    }
                    catch (InvalidOperationException oErr)
                    {
                        throw new Exception("@TransactionHandler - " + oErr.Message);
                    }
                    break;

                case TransactionType.Commit:  //commit the transaction
                    if (null != oTransaction.Connection)
                    {
                        try
                        {
                            oTransaction.Commit();
                            mblTransaction = false;
                        }
                        catch (InvalidOperationException oErr)
                        {
                            throw new Exception("@TransactionHandler - " + oErr.Message);
                        }
                    }
                    break;

                case TransactionType.Rollback:  //rollback the transaction
                    try
                    {
                        if (mblTransaction)
                        {
                            oTransaction.Rollback();
                        }
                        mblTransaction = false;
                    }
                    catch (InvalidOperationException oErr)
                    {
                        throw new Exception("@TransactionHandler - " + oErr.Message);
                    }
                    break;
            }

        }

        #endregion

        #region COMMANDS

        #region PARAMETERLESS METHODS


        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString = S_CONNECTION;
                oConnection.Open();
                oConnectionState = ConnectionState.Open;
            }

            if (null == oCommand)
                oCommand = oFactory.CreateCommand();

            oCommand.Connection = oConnection;
            oCommand.CommandText = cmdText;
            oCommand.CommandType = cmdType;

            if (blTransaction)
                oCommand.Transaction = oTransaction;
        }

        #endregion

        #region OBJECT BASED PARAMETER ARRAY


        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString = S_CONNECTION;
                oConnection.Open();
                oConnectionState = ConnectionState.Open;
            }

            if (null == oCommand)
                oCommand = oFactory.CreateCommand();

            oCommand.Connection = oConnection;
            oCommand.CommandText = cmdText;
            oCommand.CommandType = cmdType;

            if (blTransaction)
                oCommand.Transaction = oTransaction;

            if (null != cmdParms)
                CreateDBParameters(cmdParms);
        }

        #endregion

        #region STRUCTURE BASED PARAMETER ARRAY


        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString = S_CONNECTION;
                oConnection.Open();
                oConnectionState = ConnectionState.Open;
            }

            oCommand = oFactory.CreateCommand();
            oCommand.Connection = oConnection;
            oCommand.CommandText = cmdText;
            oCommand.CommandType = cmdType;

            if (blTransaction)
                oCommand.Transaction = oTransaction;

            if (null != cmdParms)
                CreateDBParameters(cmdParms);
        }

        #endregion

        #endregion

        #region PARAMETER METHODS

        #region OBJECT BASED


        private void CreateDBParameters(object[,] colParameters)
        {
            for (int i = 0; i < colParameters.Length / 2; i++)
            {
                oParameter = oCommand.CreateParameter();
                oParameter.ParameterName = colParameters[i, 0].ToString();
                oParameter.Value = colParameters[i, 1];
                oCommand.Parameters.Add(oParameter);
            }
        }

        #endregion

        #region STRUCTURE BASED


        private void CreateDBParameters(Parameters[] colParameters)
        {
            for (int i = 0; i < colParameters.Length; i++)
            {
                Parameters oParam = (Parameters)colParameters[i];

                oParameter = oCommand.CreateParameter();
                oParameter.ParameterName = oParam.ParamName;
                oParameter.Value = oParam.ParamValue;
                oParameter.DbType = oParam.ParamType;
                oParameter.Direction = oParam.ParamDirection;
                oCommand.Parameters.Add(oParameter);

            }
        }

        #endregion

        #endregion

        #region EXCEUTE METHODS

        #region PARAMETERLESS METHODS


        public int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            try
            {
                Loghelper.WriteLog("SQL语句： " + cmdText);
                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText);
                return oCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Loghelper.WriteLog("错误： " + ex.Message.ToString());
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }


        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText)
        {
            try
            {
                PrepareCommand(blTransaction, cmdType, cmdText);
                int val = oCommand.ExecuteNonQuery();

                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }

        #endregion

        #region OBJECT BASED PARAMETER ARRAY


        public int ExecuteNonQuery(CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(mblTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }


        public int ExecuteNonQuery(CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteNonQuery(cmdType, cmdText, cmdParms, true);
        }


        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }


        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteNonQuery(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #region STRUCTURE BASED PARAMETER ARRAY


        public int ExecuteNonQuery(CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {
                EstablishFactoryConnection();
                PrepareCommand(mblTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }


        public int ExecuteNonQuery(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteNonQuery(cmdType, cmdText, cmdParms, true);
        }


        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }


        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteNonQuery(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #endregion

        #region READER METHODS

        #region PARAMETERLESS METHODS


        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                EstablishFactoryConnection();
                PrepareCommand(mblTransaction, cmdType, cmdText);

                DbDataReader dr = oCommand.ExecuteReader(CommandBehavior.CloseConnection);

                oCommand.Parameters.Clear();
                return dr;
            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                {
                    oCommand.Dispose();
                }
            }
        }

        #endregion

        #region OBJECT BASED PARAMETER ARRAY


        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, object[,] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work

            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                DbDataReader dr = oCommand.ExecuteReader(CommandBehavior.CloseConnection);
                oCommand.Parameters.Clear();
                return dr;

            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }

        #endregion

        #region STRUCTURE BASED PARAMETER ARRAY


        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }

        #endregion

        #endregion

        #region ADAPTER METHODS

        #region PARAMETERLESS METHODS


        public DataSet DataAdapter(CommandType cmdType, string cmdText)
        {
            Loghelper.WriteLog("SQL语句： " + cmdText);
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(mblTransaction, cmdType, cmdText);

                dda.SelectCommand = oCommand;
                DataSet ds = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                Loghelper.WriteLog("错误： " + ex.Message.ToString());
                //return null;
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #region OBJECT BASED PARAMETER ARRAY


        public DataSet DataAdapter(CommandType cmdType, string cmdText, object[,] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(mblTransaction, cmdType, cmdText, cmdParms);

                dda.SelectCommand = oCommand;
                DataSet ds = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #region STRUCTURE BASED PARAMETER ARRAY


        public DataSet DataAdapter(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(mblTransaction, cmdType, cmdText, cmdParms);

                dda.SelectCommand = oCommand;
                DataSet ds = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #endregion

        #region SCALAR METHODS

        #region PARAMETERLESS METHODS


        public object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            try
            {
                EstablishFactoryConnection();

                PrepareCommand(false, cmdType, cmdText);

                object val = oCommand.ExecuteScalar();

                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #region OBJECT BASED PARAMETER ARRAY


        public object ExecuteScalar(CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }


        public object ExecuteScalar(CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteScalar(cmdType, cmdText, cmdParms, true);
        }


        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }


        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteScalar(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #region STRUCTURE BASED PARAMETER ARRAY


        public object ExecuteScalar(CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {
                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }


        public object ExecuteScalar(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteScalar(cmdType, cmdText, cmdParms, true);
        }


        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }


        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteScalar(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #endregion

    }
}


