﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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
            Log.Info(String.Format("Login. Username: {0}", username));

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
                Log.Debug(String.Format("Salt: {0}, EncPassword: {1}", user.Salt, encPassword));
                user = OracleDb.Instance.CheckUser(username, encPassword);

                if (user == null)
                {
                    result.Code = ResultCodes.InvalidUsernameOrPassword;
                    Log.Error("Login failed. Password: " + password);

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
        public User GetUser(String sid, String username)
        {
            Log.Info(String.Format("SID: {0}, userInfo: {1}", sid, username));

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditUsers))
                {
                    throw new Exception("No priv");
                }
                var user = OracleDb.Instance.GetUser(username);
                CleanCredentials(ref user);
                return user;
            }
            catch (Exception ex)
            {
                Log.ErrorException(ex.Message, ex);
            }

            return null;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public User GetUserById(String sid, int id)
        {
            Log.Info(String.Format("SID: {0}, userInfo: {1}", sid, id));

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditUsers))
                {
                    throw new Exception("No priv");
                }

                var user = OracleDb.Instance.GetUser(id);
                CleanCredentials(ref user);
                return user;
            }
            catch (Exception ex)
            {
                Log.ErrorException(ex.Message, ex);
            }

            return null;
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

                String salt = String.Empty;
                String encPass = String.Empty;

                if (!String.IsNullOrEmpty(userInfo.Password))
                {
                    salt = Wrapper.GenerateSalt();
                    encPass = Wrapper.ComputeHash(userInfo.Password, salt);
                }

                Log.Debug(String.Format("Salt: {0}, EncPassword: {1}", salt, encPass));

                if (!IsAlphaNum(userInfo.Username))
                {
                    result.Code = ResultCodes.InvalidParameters;
                    throw new Exception(result.Description + " " + userInfo.Username);
                }

                OracleDb.Instance.SaveUser(userInfo.Id > 0 ? (int?)userInfo.Id : null, userInfo.Username,
                                           encPass,
                                           salt);

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

                if (session.Session.User.Id == id)
                {
                    throw new Exception("We don't accept selfkill");
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
        public StandardResult SaveUserBranch(String sid, int userId, int[] branchId)
        {
            Log.Info(String.Format("SID: {0}, userId: {1}, branchId: {2}", sid, userId, branchId));
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

                OracleDb.Instance.SaveUserToBranches(userId, branchId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveUserRole(String sid, int userId, int[] roles)
        {
            Log.Info(String.Format("SID: {0}, userId: {1}, roles: {2}", sid, userId, roles));
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

                OracleDb.Instance.SaveUserToRoles(userId, roles);
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
        public ListUsersResult ListUsers(String sid, UsersColumns sortColumn, SortType sortType)
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

                result.Users = OracleDb.Instance.ListUsers(sortColumn, sortType);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListUsersResult ListUsersByUsername(String sid, String username, UsersColumns sortColumn, SortType sortType)
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

                result.Users = OracleDb.Instance.ListUsers(username, sortColumn, sortType);
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
        public ListTerminalsResult ListTerminals(string sid, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage)
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

                int count;
                result.Terminals = OracleDb.Instance.ListTerminals(sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListTerminalsResult ListTerminalsByTerminalStatus(string sid, int statusId, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage)
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

                int count;
                result.Terminals = OracleDb.Instance.ListTerminals(statusId, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListTerminalsResult ListTerminalsByBranchId(string sid, int id, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage)
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

                int count;
                result.Terminals = OracleDb.Instance.ListTerminalsByBranchId(id, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListTerminalsResult ListTerminalsByType(string sid, int type, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage)
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

                int count;
                result.Terminals = OracleDb.Instance.ListTerminalsByType(type, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public SaveTerminalResult SaveTerminal(String sid, Terminal terminal)
        {
            Log.Info(String.Format("SID: {0}, Terminal: {1}", sid, terminal));
            var result = new SaveTerminalResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditTerminal))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Mac = Wrapper.GetRandomMac();
                terminal.TmpKey = Encoding.ASCII.GetBytes(result.Mac);

                result.Id = OracleDb.Instance.SaveTerminal(session.Session.User.Id, terminal);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public SaveTerminalResult DeleteTerminal(String sid, int terminalId)
        {
            Log.Info(String.Format("SID: {0}, Terminal: {1}", sid, terminalId));
            var result = new SaveTerminalResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditTerminal))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.DeleteTerminal(terminalId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public TerminalInfoResult GetTerminal(String sid, int terminalId)
        {
            Log.Info(String.Format("GetTerminalInfo. SID: {0}, terminalId: {1}", sid, terminalId));

            var result = new TerminalInfoResult();

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

                var terminal = OracleDb.Instance.GetTerminal(terminalId);
                if (terminal != null)
                {
                    result.Code = ResultCodes.Ok;
                    result.Terminal = terminal;
                }
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListEncashmentResult ListEncashment(string sid, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage)
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

                int count;
                result.Encashments = OracleDb.Instance.ListEncashment(sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListEncashmentResult ListEncashmentByTerminal(string sid, int terminalId, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}, terminalId: {1}", sid, terminalId));
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

                int count;
                result.Encashments = OracleDb.Instance.ListEncashment(terminalId, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListEncashmentResult ListEncashmentByBranchId(string sid, int branchId, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}, branchId: {1}", sid, branchId));
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

                int count;
                result.Encashments = OracleDb.Instance.ListEncashmentBranchId(branchId, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }


        [OperationBehavior(AutoDisposeParameters = true)]
        public ListEncashmentResult ListEncashmentByAllParams(string sid, int terminalId, int branchId, DateTime from, DateTime to, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}, terminalId: {1}, branchId: {2}, from: {3}, to: {4}, Min: {5}", sid, terminalId, branchId, from, to, DateTime.MinValue));
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

                int count;
                result.Encashments = OracleDb.Instance.ListEncashment(terminalId, branchId, from, to, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListEncashmentResult ListEncashmentByDate(string sid, DateTime from, DateTime to, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}, from: {1}: to: {2}", sid, from, to));
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

                int count;
                result.Encashments = OracleDb.Instance.ListEncashment(from, to, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public CurrencyResult GetCurrency(String sid, string id)
        {
            Log.Info(String.Format("SID: {0}, ID: {1}", sid, id));
            var result = new CurrencyResult();

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

                result.Currency = OracleDb.Instance.GetCurrencies(id);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public EncashmentResult GetEncashment(String sid, int id)
        {
            Log.Info(String.Format("SID: {0}, ID: {1}", sid, id));
            var result = new EncashmentResult();

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

                result.Encashment = OracleDb.Instance.GetEncashment(id);
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
        public ListProductHistoryResult ListProductHistory(string sid, ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistoryResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                int count;
                result.Histories = OracleDb.Instance.ListProductHistory(sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductHistoryResult ListProductHistoryByDate(string sid, DateTime from, DateTime to, ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistoryResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                int count;
                result.Histories = OracleDb.Instance.ListProductHistory(from, to, sortColumn, sortType, rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductHistoryResult ListProductHistoryByTerminalId(string sid, DateTime from, DateTime to, int terminalId, ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistoryResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                int count;
                result.Histories = OracleDb.Instance.ListProductHistory(from, to, terminalId, sortColumn, sortType,
                                                                        rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductHistoryResult ListProductHistoryByEncashmentId(string sid, DateTime from, DateTime to, int encashmentId, ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistoryResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                int count;
                result.Histories = OracleDb.Instance.ListProductHistoryByEncashmentId(from, to, encashmentId, sortColumn, sortType,
                                                                        rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductHistoryResult ListProductHistoryByProductId(string sid, DateTime from, DateTime to, int productId, ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistoryResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                int count;
                result.Histories = OracleDb.Instance.ListProductHistoryByProduct(from, to, productId, sortColumn, sortType,
                                                                        rowNum, perPage, out count);
                result.Count = count;
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListProductHistoryResult ListProductHistoryByTransactionId(string sid, string transactionId, ProductHistoryColumns sortColumn, SortType sortType)
        {
            Log.Info(String.Format("SID: {0}", sid));
            var result = new ListProductHistoryResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Histories = OracleDb.Instance.ListProductHistory(transactionId, sortColumn, sortType);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCurrenciesResult ListCurrencies(String sid, CurrencyColumns sortColumn, SortType sortType)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListCurrenciesResult();

            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                //if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewCurrency))
                //{
                //    result.Code = ResultCodes.NoPriv;
                //    throw new Exception("No priv");
                //}

                result.Currencies = OracleDb.Instance.ListCurrencies(sortColumn, sortType);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult UpdateProfile(String sid, string newPassword)
        {
            Log.Info(String.Format("SID: {0}, NewPassword: {1}", sid, newPassword));

            var result = new StandardResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                string salt = Wrapper.GenerateSalt();
                string encPass = Wrapper.ComputeHash(newPassword, salt);

                OracleDb.Instance.SaveUser(session.Session.User.Id, session.Session.User.Username, encPass, salt);

                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBanknotesResult GetBanknotesByTerminal(String sid, int terminalId)
        {
            Log.Info(String.Format("SID: {0}, terminalId: {1}", sid, terminalId));

            var result = new ListBanknotesResult();
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

                result.Banknotes = OracleDb.Instance.ListBanknotesByTerminalIdNotEncashed(terminalId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBanknotesResult GetBanknotesByEncashment(String sid, int encashmentId)
        {
            Log.Info(String.Format("SID: {0}, encashmentId: {1}", sid, encashmentId));

            var result = new ListBanknotesResult();
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

                result.Banknotes = OracleDb.Instance.ListBanknotesByEncashmentId(encashmentId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBanknotesResult GetBanknotesByHistory(String sid, int historyId)
        {
            Log.Info(String.Format("SID: {0}, encashmentId: {1}", sid, historyId));

            var result = new ListBanknotesResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewProductsHistory))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Banknotes = OracleDb.Instance.GetBanknotesByHistoryId(historyId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckFieldTypeResult ListCheckFieldTypes(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListCheckFieldTypeResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Types = OracleDb.Instance.ListCheckFieldTypes();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckTypeResult ListCheckTypes(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListCheckTypeResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Types = OracleDb.Instance.ListCheckTypes();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckTemplateResult ListCheckTemplates(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListCheckTemplateResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor) && !HasPriv(session.Session.User.RoleFields, RoleSections.ViewEncashment))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Templates = OracleDb.Instance.ListCheckTemplate();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public CheckField GetCheckField(String sid, int id)
        {
            Log.Info(String.Format("SID: {0}", sid));

            CheckField result = null;
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    throw new Exception("Invalid session");
                }

                //if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                //{
                //    throw new Exception("No priv");
                //}

                result = OracleDb.Instance.GetCheckField(id);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckTemplateResult GetCheckTemplate(String sid, int id)
        {
            Log.Info(String.Format("SID: {0}, Id: {1}", sid, id));

            var result = new ListCheckTemplateResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Templates.Add(OracleDb.Instance.GetCheckTemplate(id));
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveCheckTemplate(String sid, CheckTemplate item)
        {
            Log.Info(String.Format("SID: {0}, Template: {1}", sid, item));

            var result = new StandardResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                int id = OracleDb.Instance.SaveCheckTemplate(item);

                List<CheckField> oldList = null;
                if (item.Id > 0)
                {
                    oldList = OracleDb.Instance.ListCheckFields(item.Id);
                    Log.Debug(EnumEx.GetStringFromArray(oldList));
                }

                //Changes
                if (oldList != null)
                {
                    //Checking for possible NullPointerException
                    foreach (var dbRow in oldList)
                    {
                        bool found = false;
                        foreach (var row in item.Fields)
                        {
                            row.CheckId = id; //setting exact check Id
                            if (row.Id == dbRow.Id)
                            {
                                found = true;
                                if ((dbRow.Image != null && dbRow.Image.Length > 0) &&
                                    (row.Image == null || row.Image.Length == 0))
                                {
                                    row.Image = dbRow.Image;
                                    Log.Debug("Saving image.. ");
                                }
                            }
                        }
                        if (!found)
                        {
                            OracleDb.Instance.DeleteCheckField(dbRow.Id);
                            Log.Debug("Deleting check field. Id is: " + dbRow.Id);
                        }
                    }
                }

                OracleDb.Instance.SaveCheckField(item.Fields);//was for saveList List now works just for item.Fields List.

                //end of changes

                result.Code = ResultCodes.Ok;
            }

            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult DeleteCheckTemplate(String sid, int id)
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

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.DeleteCheckTemplate(id);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult ActivateCheckTemplate(String sid, int id, bool activationStatus)
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

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.EditCheckConstructor))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.ActivateCheckTemplate(id, activationStatus);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBranchesResult ListBranches(String sid, BranchColumns column, SortType sortType)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListBranchesResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewBranches))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Branches = OracleDb.Instance.ListBranches(column, sortType);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBranchesResult GetBranch(String sid, int id)
        {
            Log.Info(String.Format("SID: {0}, Id: {1}", sid, id));

            var result = new ListBranchesResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewBranches))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Branches = new List<Branch> { OracleDb.Instance.GetBranch(id) };
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult SaveBranch(String sid, Branch branch)
        {
            Log.Info(String.Format("SID: {0}, Branch: {1}", sid, branch));

            var result = new StandardResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewBranches))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                OracleDb.Instance.SaveBranch(branch.Id, branch.Name, session.Session.User.Id);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckTemplateResult GetCheckTemplates(int id)
        {
            var result = new ListCheckTemplateResult();
            try
            {
                result.Templates.Add(OracleDb.Instance.GetCheckTemplate(id));
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListTerminalStatusResult ListTerminalStatusCode(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListTerminalStatusResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewBranches))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.Statuses = OracleDb.Instance.ListTerminalStatusCode();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListTerminalTypeResult ListTerminalTypes(String sid)
        {
            Log.Info(String.Format("SID: {0}", sid));

            var result = new ListTerminalTypeResult();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                if (!HasPriv(session.Session.User.RoleFields, RoleSections.ViewBranches))
                {
                    result.Code = ResultCodes.NoPriv;
                    throw new Exception("No priv");
                }

                result.TerminalTypes = OracleDb.Instance.ListTerminalType();
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBanknoteSummary ListBanknoteSummaryByTerminalId(String sid, int terminalId)
        {
            Log.Info(String.Format("SID: {0}, ID: {1}", sid, terminalId));

            var result = new ListBanknoteSummary();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                result.Summaries = OracleDb.Instance.ListSummaryBanknotes(terminalId);
                foreach (var item in result.Summaries)
                {
                    Log.Debug(item);
                }
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBanknoteSummary ListBanknoteSummaryByHistoryId(String sid, int historyId)
        {
            Log.Info(String.Format("SID: {0}, ID: {1}", sid, historyId));

            var result = new ListBanknoteSummary();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                result.Summaries = OracleDb.Instance.ListSummaryBanknotesHistoryId(historyId);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListBanknoteSummary ListBanknoteSummaryByEncashmentId(String sid, int encashmentId)
        {
            Log.Info(String.Format("SID: {0}, ID: {1}", sid, encashmentId));

            var result = new ListBanknoteSummary();
            try
            {
                var session = CheckSession(sid);
                if (session.Code != ResultCodes.Ok)
                {
                    result.Code = session.Code;
                    throw new Exception("Invalid session");
                }

                result.Summaries = OracleDb.Instance.ListSummaryBanknotesEncashmentId(encashmentId);
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

        private bool IsAlphaNum(String value)
        {
            var rgx = new Regex(@"^[a-z0-9]+$");

            if (rgx.IsMatch(value))
            {
                return true;
            }
            return false;
        }
    }
}
