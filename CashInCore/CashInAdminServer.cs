using System;
using System.Collections.Generic;
using System.ServiceModel;
using Containers;
using Containers.Admin;
using Containers.Enums;
using Db;
using NLog;
using crypto;

namespace CashInCore
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerSession,
        UseSynchronizationContext = true,
        ConfigurationName = "CashInCore.CashInAdminServer")]
    public class CashInAdminServer : ICashInAdminServer
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
                var user = OracleDb.Instance.GetUser(username);

                if (user == null)
                {
                    result.Code = ResultCodes.InvalidUsernameOrPassword;
                    Log.Error("Username not found");

                    return result;
                }

                var encPassword = Wrapper.ComputeHash(password, user.Salt);
                user = OracleDb.Instance.CheckUser(username, encPassword);

                if (user == null)
                {
                    result.Code = ResultCodes.InvalidUsernameOrPassword;
                    Log.Error("Login failed");

                    return result;
                }

                CleanCredentials(ref user);
                result.UserInfo = user;
                result.Sid = Guid.NewGuid().ToString();

                OracleDb.Instance.StartSession(result.Sid, user.Id);

                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public SessionResult CheckSession(String sid)
        {
            var result = new SessionResult();
            try
            {
                var session = OracleDb.Instance.CheckSession(sid);

                if (session == null)
                {
                    result.Code = ResultCodes.InvalidSession;
                    throw new Exception("Session not found");
                }

                OracleDb.Instance.UpdateSession(sid, session.User.Id);
                result.Session = session;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveUser(String sid, User userInfo)
        {
            Log.Info(String.Format("SID: {0}, userInfo: {1}", sid, userInfo));
            var result = new StandardResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditUsers))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.SaveUser(userInfo.Id > 0 ? (int?)userInfo.Id : null, userInfo.Username,
                                           userInfo.Password,
                                           String.IsNullOrEmpty(userInfo.Password)
                                               ? Wrapper.GenerateSalt()
                                               : String.Empty);

                result.Code = ResultCodes.Ok;

            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult DeleteUser(String sid, int id)
        {
            Log.Info(String.Format("SID: {0}, Id: {1}", sid, id));
            var result = new StandardResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditUsers))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.DeleteUser(id);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SetStatusCode(String sid, int terminalId, int statusCode)
        {
            Log.Info(String.Format("SID: {0}, terminalId: {1}, statusCode: {2}", sid, terminalId, statusCode));
            var result = new StandardResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.SendTerminalStatus))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.SetTerminalStatusCode(session.Session.User.Id, terminalId, statusCode);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveUserRole(String sid, int userId, int roleId)
        {
            Log.Info(String.Format("SID: {0}, userId: {1}, roleId: {2}", sid, userId, roleId));
            var result = new StandardResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditUsers))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.SetUserRole(userId, roleId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveCurrency(string sid, Currency currency)
        {
            Log.Info(String.Format("SID: {0}, currency: {1}", sid, currency));
            var result = new StandardResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCurrency))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.SaveCurrency(currency.Id, currency.IsoName, currency.Name, currency.DefaultCurrency, session.Session.User.Id);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveCurrencyRate(String sid, string currency, decimal rate)
        {
            Log.Info(String.Format("SID: {0}, currency: {1}, rate: {2}", sid, currency, rate));
            var result = new StandardResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCurrency))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.SaveCurrencyRate(currency, rate, session.Session.User.Id);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListUsersResult ListUsers(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListUsersResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewUsers))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Users = OracleDb.Instance.ListUsers();                
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListRolesResult ListRoles(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListRolesResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewUsers))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Roles = OracleDb.Instance.ListRoles();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListTerminalsResult ListTerminals(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListTerminalsResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewTerminal))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Terminals = OracleDb.Instance.ListTerminals();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListEncashmentResult ListEncashment(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListEncashmentResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewEncashment))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Encashments = OracleDb.Instance.ListEncashment();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductsResult ListProducts(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductsResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProducts))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                
                result.Products = OracleDb.Instance.ListProducts();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductHistory ListProductHistory(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistory();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProducts))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                // TODO : Add method!
                // TODO : 
                // TODO : 
                // TODO : 
                //result.History = OracleDb.Instance.ListProducts();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        private bool HasPriv(IEnumerable<AccessRole> roles, String requiredPriv)
        {
            foreach (var role in roles)
            {
                if (role.Section == requiredPriv)
                {
                    return true;
                }
            }

            return false;
        }

        private void CleanCredentials(ref User user)
        {
            user.Salt = String.Empty;
            user.Password = String.Empty;
        }
    }
}
