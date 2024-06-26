﻿using System;
using NLog;
using Oracle.DataAccess.Client;

namespace Db
{
    public partial class OracleDb : MarshalByRefObject
    {
        private static string _ConnectionString;
        //private OracleConnection _OraCon;
        private static string _DbName = String.Empty;
        private static volatile OracleDb _Instance;
        private static readonly object _SyncRoot = new object();
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        //private bool IsAlive()
        //{
        //    try
        //    {
        //        using (OracleCommand cmd = _OraCon.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT 1 FROM dual";
        //            object o = cmd.ExecuteScalar();
        //        }
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Log.ErrorException(e.Message, e);
        //    }

        //    return false;
        //}

        public static void Init(string user, string pass, string dbase)
        {
            if (_ConnectionString == null)
            {
                _DbName = dbase;
                _ConnectionString = "User ID=" + user + ";Data Source=" + dbase + ";Password=" + pass +
                    ";Min Pool Size=1;Max Pool Size=50;Pooling=True;Validate Connection=true;Connection Lifetime=300;Connection Timeout=300";
            }
        }

        public string GetDbName()
        {
            return _DbName;
        }

        private OracleDb()
        {
            //try
            //{
            //    //_OraCon = new OracleConnection(_ConnectionString);
            //    //_OraCon.Open();
            //}
            //catch (OracleException e)
            //{
            //    throw new Exception(e.Message);
            //}
        }

        public static OracleDb Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new OracleDb();
                        }
                    }
                }

                return _Instance;
            }
        }

        public bool CheckConnection()
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM dual";
                    object o = cmd.ExecuteScalar();
                }

                return true;
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
           
            return false;
        }

        #region IDisposable Members

        public void Dispose()
        {
            //if (_OraCon != null && _OraCon.State == System.Data.ConnectionState.Open)
            //{
            //    _OraCon.Close();
            //}
        }

        #endregion
    }
}