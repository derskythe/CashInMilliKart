using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Containers;
using Containers.Admin;
using Containers.Enums;
using NLog;
using crypto;

namespace CashInCore
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerSession,
        UseSynchronizationContext = true,
        ConfigurationName = "CashInCore.CashInAdminServer")]
    public class CashInAdminServer
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        [OperationBehavior(AutoDisposeParameters = true)]
        public LoginResult Login(string username, string password)
        {
            Log.Info("Login");

            var result = new LoginResult();

            try
            {
                var user = Db.OracleDb.Instance.GetUser(username);

                if (user == null)
                {
                    result.Code = ResultCodes.InvalidUsernameOrPassword;
                    Log.Error("Username not found");

                    return result;
                }

                var encPassword = Wrapper.ComputeHash(password, user.Salt);
                user = Db.OracleDb.Instance.CheckUser(username, encPassword);

                if (user == null)
                {
                    result.Code = ResultCodes.InvalidUsernameOrPassword;
                    Log.Error("Login failed");

                    return result;
                }

                CleanCredentials(ref user);
                result.UserInfo = user;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        private SessionResult CheckSession(String sid)
        {
            var result = new SessionResult();
            try
            {
                var session = Db.OracleDb.Instance.CheckSession(sid);

                if (session == null)
                {
                    result.Code = ResultCodes.InvalidSession;
                    throw new Exception("Session not found");
                }

                result.Session = session;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        private void CleanCredentials(ref User user)
        {
            user.Salt = String.Empty;
            user.Password = String.Empty;
        }
    }
}
