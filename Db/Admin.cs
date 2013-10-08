using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Containers;
using Containers.Admin;
using Containers.Enums;
using Db.dsTableAdapters;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Db
{
    public partial class OracleDb
    {
        public void StartSession(string sid, int userId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText = "begin main.start_session(v_sid => :v_sid, v_user_id => :v_user_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_sid", OracleDbType.Varchar2, ParameterDirection.Input).Value = sid;
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void UpdateSession(string sid, int userId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText = "begin main.update_session(v_sid => :v_sid, v_user_id => :v_user_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_sid", OracleDbType.Varchar2, ParameterDirection.Input).Value = sid;
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SaveUserToBranches(int userId, int[] branches)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                var pUserId = new OracleParameter();
                var pBranches = new OracleParameter();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "main.save_users_to_branches";
                    cmd.CommandType = CommandType.StoredProcedure;

                    pUserId.OracleDbType = OracleDbType.Int32;
                    pBranches.OracleDbType = OracleDbType.Int32;

                    pBranches.CollectionType = OracleCollectionType.PLSQLAssociativeArray;

                    pUserId.Value = userId;
                    pBranches.Value = branches;

                    pBranches.Size = branches.Length;

                    cmd.Parameters.Add(pUserId);
                    cmd.Parameters.Add(pBranches);

                    cmd.ExecuteNonQuery();
                }
                pUserId.Dispose();
                pBranches.Dispose();

            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SaveUserToRoles(int userId, int[] roles)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                var pUserId = new OracleParameter();
                var pRoles = new OracleParameter();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "main.save_users_to_roles";
                    cmd.CommandType = CommandType.StoredProcedure;

                    pUserId.OracleDbType = OracleDbType.Int32;
                    pRoles.OracleDbType = OracleDbType.Int32;

                    pRoles.CollectionType = OracleCollectionType.PLSQLAssociativeArray;

                    pUserId.Value = userId;
                    pRoles.Value = roles;

                    pRoles.Size = roles.Length;

                    cmd.Parameters.Add(pUserId);
                    cmd.Parameters.Add(pRoles);

                    cmd.ExecuteNonQuery();
                }

                pUserId.Dispose();
                pRoles.Dispose();
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SaveUser(int? userId, string userName, string password, string salt)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText =
                    "begin main.save_user(v_id => :v_id, v_username => :v_username, v_password => :v_password, v_salt => :v_salt); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
                    cmd.Parameters.Add("v_username", OracleDbType.Varchar2, ParameterDirection.Input).Value = userName;
                    cmd.Parameters.Add("v_password", OracleDbType.Varchar2, ParameterDirection.Input).Value = password;
                    cmd.Parameters.Add("v_salt", OracleDbType.Varchar2, ParameterDirection.Input).Value = salt;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void DeleteUser(int userId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText = "begin main.delete_user(v_id => :v_id); end;";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SetTerminalStatusCode(int userId, int terminalId, int status)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText =
                    "begin main.set_terminal_status(v_user_id => :v_user_id, v_terminal_id => :v_terminal_id, v_status => :v_status); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_status", OracleDbType.Int32, ParameterDirection.Input).Value = status;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SetUserRole(int userId, int roleId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText = "begin main.set_user_role(v_user_id => :v_user_id, v_role_id => :v_role_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
                    cmd.Parameters.Add("v_role_id", OracleDbType.Int32, ParameterDirection.Input).Value = roleId;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public int SaveTerminal(int userId, Terminal terminal)
        {
            OracleConnection connection = null;
            object result;

            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText =
                    "begin main.save_terminal(v_id => :v_id, v_name => :v_name, v_address => :v_address, v_identity_name => :v_identity_name, v_ip => :v_ip, v_tmp_key => :v_tmp_key, v_user_id => :v_user_id, v_branch_id => :v_branch_id, v_type => :v_type, v_return_id => :v_return_id); end;";


                using (var cmd = new OracleCommand(cmdText, connection))
                {

                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminal.Id > 0
                                                                                                         ? (int?)
                                                                                                           terminal.Id
                                                                                                         : null;
                    cmd.Parameters.Add("v_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = terminal.Name;
                    cmd.Parameters.Add("v_address", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        terminal.Address;
                    cmd.Parameters.Add("v_identity_name", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        terminal.IdentityName;
                    cmd.Parameters.Add("v_ip", OracleDbType.Varchar2, ParameterDirection.Input).Value = terminal.Ip;
                    cmd.Parameters.Add("v_tmp_key", OracleDbType.Blob, ParameterDirection.Input).Value = terminal.TmpKey;
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
                    cmd.Parameters.Add("v_branch_id", OracleDbType.Int32, ParameterDirection.Input).Value =
                        terminal.BranchId;
                    cmd.Parameters.Add("v_type", OracleDbType.Int32, ParameterDirection.Input).Value = terminal.Type;
                    cmd.Parameters.Add("v_return_id", OracleDbType.Int32, ParameterDirection.Output);

                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["v_return_id"].Value;
                }


            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result == null ? 0 : ((OracleDecimal)(result)).ToInt32();
        }

        public void DeleteTerminal(int terminal)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.delete_terminal(v_id => :v_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminal;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SaveCurrency(string id, string isoName, string name, bool defaultCurrency, int userId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_currency(v_id => :v_id, v_iso_name => :v_iso_name, v_name => :v_name, v_default_currency => :v_default_currency, v_user_id => :v_user_id); end;";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Varchar2, ParameterDirection.Input).Value = id;
                    cmd.Parameters.Add("v_iso_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = isoName;
                    cmd.Parameters.Add("v_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = name;
                    cmd.Parameters.Add("v_default_currency", OracleDbType.Int16, ParameterDirection.Input).Value =
                        defaultCurrency ? 1 : 0;
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void SaveCurrencyRate(string currency, decimal rate, int userId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_currency_rate(v_currency_id => :v_currency_id, v_rate => :v_rate, v_user_id => :v_user_id); end; ";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_currency_id", OracleDbType.Varchar2, ParameterDirection.Input).Value = currency;
                    cmd.Parameters.Add("v_rate", OracleDbType.Decimal, ParameterDirection.Input).Value = rate;
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public ds.V_ROLES_TO_USERSRow[] ListRolesToUsersRoleId(int roleId)
        {
            OracleConnection connection = null;
            var result = new List<ds.V_ROLES_TO_USERSRow>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_ROLES_TO_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_ROLES_TO_USERSDataTable())
                    {
                        adapter.FillByRoleId(table, roleId);

                        foreach (ds.V_ROLES_TO_USERSRow row in table.Rows)
                        {
                            result.Add(row);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();

        }

        public List<AccessRole> ListRolesToUsersUserId(decimal userId)
        {
            OracleConnection connection = null;
            var result = new List<AccessRole>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_ROLES_TO_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_ROLES_TO_USERSDataTable())
                    {
                        adapter.FillByUserId(table, userId);
                        foreach (ds.V_ROLES_TO_USERSRow row in table.Rows)
                        {
                            result.Add(Convertor.ToAccessRole(row));
                        }
                    }
                }

            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public User CheckUser(string username, string password)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_USERSDataTable())
                    {
                        adapter.FillByActive(table, username, password);

                        foreach (ds.V_LIST_USERSRow row in table.Rows)
                        {
                            var roles = ListRolesToUsersUserId(row.ID);
                            var branches = ListBranchesByUser(row.ID);
                            var user = Convertor.ToUser(row, roles, branches);
                            return user;
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public User GetUser(int id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_USERSDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_LIST_USERSRow row in table.Rows)
                        {
                            var roles = ListRolesToUsersUserId(row.ID);
                            var branches = ListBranchesByUser(row.ID);
                            return Convertor.ToUser(row, roles, branches);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public User GetUser(decimal id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_USERSDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_LIST_USERSRow row in table.Rows)
                        {
                            var roles = ListRolesToUsersUserId(row.ID);
                            var branches = ListBranchesByUser(row.ID);
                            return Convertor.ToUser(row, roles, branches);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public User GetUser(string username)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_USERSDataTable())
                    {
                        adapter.FillByUsername(table, username);

                        foreach (ds.V_LIST_USERSRow row in table.Rows)
                        {
                            var roles = ListRolesToUsersUserId(row.ID);
                            var branches = ListBranchesByUser(row.ID);
                            return Convertor.ToUser(row, roles, branches);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public List<Currency> ListCurrencies(CurrencyColumns sortColumn, SortType sortType)
        {
            OracleConnection connection = null;
            var result = new List<Currency>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var column = String.Empty;
                switch (sortColumn)
                {
                    case CurrencyColumns.Id:
                        column = "t.ID";
                        break;

                    case CurrencyColumns.IsoName:
                        column = "t.ISO_NAME";
                        break;

                    case CurrencyColumns.Name:
                        column = "t.NAME";
                        break;

                    case CurrencyColumns.Rate:
                        column = "t.rate";
                        break;
                }

                var cmdText = String.Format("select * from V_LIST_CURRENCIES t ORDER BY {0} {1}", column,
                                            sortType.ToString());


                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_CURRENCIESDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_CURRENCIESRow row in table.Rows)
                            {
                                result.Add(Convertor.ToCurrency(row));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        //public User[] ListUsers()
        //{
        //    CheckConnection();

        //    var adapter = new V_LIST_USERSTableAdapter { Connection = connection, BindByName = true };

        //    var table = new ds.V_LIST_USERSDataTable();
        //    adapter.Fill(table);

        //    var result = new List<User>();
        //    foreach (ds.V_LIST_USERSRow row in table.Rows)
        //    {
        //        var roles = ListRolesToUsersUserId(row.ID);
        //        var user = Convertor.ToUser(row, roles);
        //        result.Add(user);
        //    }

        //    return result.ToArray();
        //}

        public User[] ListUsers(UsersColumns sortColumn, SortType sortType)
        {
            OracleConnection connection = null;
            var result = new List<User>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var column = String.Empty;
                switch (sortColumn)
                {
                    case UsersColumns.Username:
                        column = "t.username";
                        break;

                    case UsersColumns.InsertDate:
                        column = "t.INSERT_DATE";
                        break;

                    case UsersColumns.UpdateDate:
                        column = "t.UPDATE_DATE";
                        break;
                }

                var cmdText = String.Format("select * from V_LIST_USERS t ORDER BY {0} {1}", column, sortType.ToString());



                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_USERSDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_USERSRow row in table.Rows)
                            {
                                var roles = ListRolesToUsersUserId(row.ID);
                                var branches = ListBranchesByUser(row.ID);
                                var user = Convertor.ToUser(row, roles, branches);
                                result.Add(user);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        public User[] ListUsers(String username, UsersColumns sortColumn, SortType sortType)
        {
            OracleConnection connection = null;
            var result = new List<User>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var column = String.Empty;
                switch (sortColumn)
                {
                    case UsersColumns.Username:
                        column = "t.username";
                        break;

                    case UsersColumns.InsertDate:
                        column = "t.INSERT_DATE";
                        break;

                    case UsersColumns.UpdateDate:
                        column = "t.UPDATE_DATE";
                        break;
                }

                var cmdText = String.Format(
                    "select * from V_LIST_USERS t WHERE t.username LIKE :username ORDER BY {0} {1}", column,
                    sortType.ToString());



                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("username", OracleDbType.Varchar2, ParameterDirection.Input).Value = username;
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_USERSDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_USERSRow row in table.Rows)
                            {
                                var roles = ListRolesToUsersUserId(row.ID);
                                var branches = ListBranchesByUser(row.ID);
                                var user = Convertor.ToUser(row, roles, branches);
                                result.Add(user);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        public AccessRole[] ListRoles()
        {
            OracleConnection connection = null;
            var result = new List<AccessRole>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();


                using (var adapter = new V_ROLESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_ROLESDataTable())
                    {
                        adapter.Fill(table);

                        foreach (ds.V_ROLESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToAccessRole(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        public UserSession CheckSession(String sid)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_ACTIVE_SESSIONSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_ACTIVE_SESSIONSDataTable())
                    {
                        adapter.FillBySid(table, sid);

                        foreach (ds.V_LIST_ACTIVE_SESSIONSRow row in table.Rows)
                        {
                            var roles = ListRolesToUsersUserId(row.USER_ID);
                            var branches = ListBranchesByUser(row.USER_ID);
                            return Convertor.ToUserSession(row, roles, branches);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public Terminal[] ListTerminals(TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Terminal>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter1 = new V_LIST_TERMINALSTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRows();
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return null;
                    }
                }

                var column = GetTerminalSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_terminals t) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage);
                //string cmdtxt = String.Format("SELECT * FROM v_list_terminals t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_TERMINALSDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
                            {
                                result.Add(Convertor.ToTerminal(item));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        public Terminal[] ListTerminals(int terminalStatus, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Terminal>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (IDbCommand command = new OracleCommand())
                {
                    command.CommandText =
                        String.Format("SELECT COUNT(*) FROM v_list_terminals t WHERE t.LAST_STATUS_TYPE = {0}",
                                      terminalStatus);
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    object rawValue = command.ExecuteScalar();
                    count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                    if (count <= 0)
                    {
                        return null;
                    }
                }

                var column = GetTerminalSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_terminals t WHERE t.LAST_STATUS_TYPE = {4}) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage, terminalStatus);
                //string cmdtxt = String.Format("SELECT * FROM v_list_terminals t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_TERMINALSDataTable())
                        {
                            adapter.Fill(table);
                            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
                            {
                                result.Add(Convertor.ToTerminal(item));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        public Terminal[] ListTerminalsByType(int type, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Terminal>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (IDbCommand command = new OracleCommand())
                {
                    command.CommandText = String.Format("SELECT COUNT(*) FROM v_list_terminals t WHERE t.TYPE = {0}",
                                                        type);
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    object rawValue = command.ExecuteScalar();
                    count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                    if (count <= 0)
                    {
                        return null;
                    }
                }

                var column = GetTerminalSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_terminals t WHERE t.TYPE = {4}) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage, type);
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_TERMINALSDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
                            {
                                result.Add(Convertor.ToTerminal(item));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        public Terminal[] ListTerminalsByBranchId(int branchId, TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Terminal>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (IDbCommand command = new OracleCommand())
                {
                    command.CommandText =
                        String.Format("SELECT COUNT(*) FROM v_list_terminals t WHERE t.BRANCH_ID = {0}",
                                      branchId);
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    object rawValue = command.ExecuteScalar();
                    count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                    if (count <= 0)
                    {
                        return null;
                    }
                }

                var column = GetTerminalSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_terminals t WHERE t.BRANCH_ID = {4}) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage, branchId);
                //string cmdtxt = String.Format("SELECT * FROM v_list_terminals t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    //cmd.Parameters.Add("rowNum", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum;
                    //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_TERMINALSDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
                            {
                                result.Add(Convertor.ToTerminal(item));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result.ToArray();
        }

        private static string GetTerminalSortColumn(TerminalColumns sortColumn)
        {
            var column = String.Empty;
            switch (sortColumn)
            {
                case TerminalColumns.Address:
                    column = "t.ADDRESS";
                    break;

                case TerminalColumns.BillsCount:
                    column = "t.bills_count";
                    break;

                case TerminalColumns.CashcodeError:
                    column = "t.LAST_CASHCODE_ERROR";
                    break;

                case TerminalColumns.CashcodeOutStatus:
                    column = "t.LAST_CASHCODE_OUT_STATUS";
                    break;

                case TerminalColumns.CashcodeStatus:
                    column = "t.LAST_CASHCODE_STATUS";
                    break;

                case TerminalColumns.CashcodeSuberror:
                    column = "t.LAST_CASHCODE_SUBERROR";
                    break;

                case TerminalColumns.CreateDate:
                    column = "t.CREATE_DATE";
                    break;

                case TerminalColumns.Id:
                    column = "t.ID";
                    break;

                case TerminalColumns.IdentityName:
                    column = "t.IDENTITY_NAME";
                    break;

                case TerminalColumns.LastUpdate:
                    column = "t.LAST_UPDATE";
                    break;

                case TerminalColumns.Name:
                    column = "t.NAME";
                    break;

                case TerminalColumns.PrinterErrorState:
                    column = "t.LAST_PRINTER_ERROR_STATE";
                    break;

                case TerminalColumns.PrinterExtErrorState:
                    column = "t.LAST_PRINTER_EXT_ERROR_STATE";
                    break;

                case TerminalColumns.PrinterStatus:
                    column = "t.LAST_PRINTER_STATUS";
                    break;

                case TerminalColumns.StatusType:
                    column = "t.LAST_STATUS_TYPE";
                    break;

                case TerminalColumns.StatusUpdate:
                    column = "t.LAST_STATUS_UPDATE";
                    break;

                case TerminalColumns.Branch:
                    column = "t.BRANCH_NAME";
                    break;
            }
            return column;
        }

        public List<Encashment> ListEncashment(EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Encashment>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter1 = new V_LIST_ENCASHMENTTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRows();
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return null;
                    }
                }

                var column = GetEncashmentSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_encashment t) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage);

                //string cmdtxt = String.Format("SELECT * FROM v_list_encashment t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    //cmd.Parameters.Add("rowNum", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum;
                    //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_ENCASHMENTDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
                            {
                                var fields = ListEncashmentCurrencies(row.ID);
                                var terminal = Convertor.ToTerminal(row);
                                var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                                result.Add(Convertor.ToEncashment(row, fields, terminal, username));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        private static string GetEncashmentSortColumn(EncashmentColumns sortColumn)
        {
            var column = String.Empty;
            switch (sortColumn)
            {
                case EncashmentColumns.InsertDate:
                    column = "t.INSERT_DATE";
                    break;

                case EncashmentColumns.TerminalId:
                    column = "t.TERMINAL_ID";
                    break;

                case EncashmentColumns.TerminalName:
                    column = "t.NAME";
                    break;

                case EncashmentColumns.Username:
                    column = "t.username";
                    break;
            }
            return column;
        }

        public List<Encashment> ListEncashment(int terminalId, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Encashment>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (IDbCommand command = new OracleCommand())
                {
                    command.CommandText =
                        String.Format("SELECT COUNT(*) FROM v_list_encashment t WHERE t.terminal_id = {0}",
                                      terminalId);
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    object rawValue = command.ExecuteScalar();
                    count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                    if (count <= 0)
                    {
                        return null;
                    }
                }

                var column = GetEncashmentSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_encashment t WHERE t.terminal_id = {4}) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage, terminalId);

                //string cmdtxt = String.Format("SELECT * FROM v_list_encashment t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_ENCASHMENTDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
                            {
                                var fields = ListEncashmentCurrencies(row.ID);
                                var terminal = Convertor.ToTerminal(row);
                                var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                                result.Add(Convertor.ToEncashment(row, fields, terminal, username));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Encashment> ListEncashmentBranchId(int id, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Encashment>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var command =
                        new OracleCommand("SELECT COUNT(*) FROM v_list_encashment t WHERE t.BRANCH_ID = :branchId")
                            {
                                CommandType = CommandType.Text,
                                Connection = connection
                            })
                {
                    command.Parameters.Add("branchId", OracleDbType.Int32, ParameterDirection.Input).Value = id;
                    object rawValue = command.ExecuteScalar();
                    count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                    if (count <= 0)
                    {
                        return null;
                    }
                }

                var column = GetEncashmentSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_encashment t WHERE t.BRANCH_ID = :branchId) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage);

                //string cmdtxt = String.Format("SELECT * FROM v_list_encashment t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    cmd.Parameters.Add("branchId", OracleDbType.Int32, ParameterDirection.Input).Value = id;
                    //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_ENCASHMENTDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
                            {
                                var fields = ListEncashmentCurrencies(row.ID);
                                var terminal = Convertor.ToTerminal(row);
                                var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                                result.Add(Convertor.ToEncashment(row, fields, terminal, username));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Encashment> ListEncashment(int terminalId, int branchId, DateTime from, DateTime to, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Encashment>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                to = to.AddDays(1);

                using (var command = new OracleCommand
                    {
                        CommandType = CommandType.Text,
                        Connection = connection
                    })
                {
                    using (var cmd = new OracleCommand { CommandType = CommandType.Text, Connection = connection })
                    {
                        const string sql = "SELECT COUNT(*) FROM v_list_encashment t";
                        var whereSql = String.Empty;

                        // Bind by name not exists!
                        if (terminalId > 0)
                        {
                            whereSql = " WHERE t.terminal_id = :encashmentId ";
                            command.Parameters.Add("encashmentId", OracleDbType.Int32, ParameterDirection.Input).Value =
                                terminalId;
                            cmd.Parameters.Add("encashmentId", OracleDbType.Int32, ParameterDirection.Input).Value =
                                terminalId;
                        }
                        if (branchId > 0)
                        {
                            if (String.IsNullOrEmpty(whereSql))
                            {
                                whereSql = " WHERE t.BRANCH_ID = :branchId ";
                            }
                            else
                            {
                                whereSql += " AND t.BRANCH_ID = :branchId ";
                            }

                            command.Parameters.Add("branchId", OracleDbType.Int32, ParameterDirection.Input).Value =
                                branchId;
                            cmd.Parameters.Add("branchId", OracleDbType.Int32, ParameterDirection.Input).Value =
                                branchId;
                        }

                        if (from != DateTime.MinValue && to != DateTime.MinValue)
                        {
                            if (String.IsNullOrEmpty(whereSql))
                            {
                                whereSql = " WHERE t.insert_date BETWEEN :dateFrom AND :dateTo ";
                            }
                            else
                            {
                                whereSql += " AND t.insert_date BETWEEN :dateFrom AND :dateTo ";
                            }

                            command.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                            command.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                            cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                            cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                        }
                        else if (from != DateTime.MinValue)
                        {
                            if (String.IsNullOrEmpty(whereSql))
                            {
                                whereSql = " WHERE t.insert_date > :dateFrom ";
                            }
                            else
                            {
                                whereSql += " AND t.insert_date > :dateFrom ";
                            }

                            command.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                            cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                        }
                        else if (to != DateTime.MinValue)
                        {
                            if (String.IsNullOrEmpty(whereSql))
                            {
                                whereSql = " WHERE t.insert_date < :dateTo ";
                            }
                            else
                            {
                                whereSql += " AND t.insert_date < :dateTo ";
                            }

                            command.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                            cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                        }

                        command.CommandText = sql + whereSql;
                        Log.Debug(sql + whereSql);

                        object rawValue = command.ExecuteScalar();
                        count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                        if (count <= 0)
                        {
                            return null;
                        }


                        var column = GetEncashmentSortColumn(sortColumn);

                        string cmdtxt =
                            String.Format(
                                "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_encashment t {4}) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                                column, sortType.ToString(), rowNum, rowNum + perPage, whereSql);
                        cmd.CommandText = cmdtxt;
                        Log.Debug(cmdtxt);

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            using (var table = new ds.V_LIST_ENCASHMENTDataTable())
                            {
                                adapter.Fill(table);

                                foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
                                {
                                    var fields = ListEncashmentCurrencies(row.ID);
                                    var terminal = Convertor.ToTerminal(row);
                                    var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                                    result.Add(Convertor.ToEncashment(row, fields, terminal, username));
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Encashment> ListEncashment(DateTime from, DateTime to, EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<Encashment>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                to = to.AddDays(1);

                using (
                    var command =
                        new OracleCommand(
                            "SELECT COUNT(*) FROM v_list_encashment t WHERE t.insert_date BETWEEN :dateFrom AND :dateTo")
                            {
                                CommandType = CommandType.Text,
                                Connection = connection
                            })
                {

                    command.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                    command.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                    object rawValue = command.ExecuteScalar();
                    count = rawValue != null ? Convert.ToInt32(rawValue) : 0;

                    if (count <= 0)
                    {
                        return null;
                    }
                }

                var column = GetEncashmentSortColumn(sortColumn);

                string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_encashment t WHERE insert_date BETWEEN :dateFrom AND :dateTo) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage);

                //string cmdtxt = String.Format("SELECT * FROM v_list_encashment t ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                    cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_LIST_ENCASHMENTDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
                            {
                                var fields = ListEncashmentCurrencies(row.ID);
                                var terminal = Convertor.ToTerminal(row);
                                var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                                result.Add(Convertor.ToEncashment(row, fields, terminal, username));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public Encashment GetEncashment(int id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                using (var adapter = new V_LIST_ENCASHMENTTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_ENCASHMENTDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
                        {
                            var fields = ListEncashmentCurrencies(row.ID);
                            var terminal = Convertor.ToTerminal(row);
                            var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;
                            var banknotes = ListBanknotesByEncashmentId(row.ID);

                            return Convertor.ToEncashment(row, fields, terminal, username, banknotes);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public List<EncashmentCurrency> ListEncashmentCurrencies(decimal id)
        {
            OracleConnection connection = null;
            var result = new List<EncashmentCurrency>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                using (
                    var adapter = new V_LIST_ENCASHMENT_CURRENCIESTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_LIST_ENCASHMENT_CURRENCIESDataTable())
                    {
                        adapter.FillByEncashmentId(table, id);

                        foreach (ds.V_LIST_ENCASHMENT_CURRENCIESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToEncashmentCurrency(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public ProductHistory GetProductHistory(long historyId)
        {
            OracleConnection connection = null;            

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_PRODUCTS_HISTORYTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                    {
                        adapter.FillById(table, historyId);

                        foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                        {
                            //var values = ListProductHistoryValues(row.ID);
                            //var banknotes = ListBanknotesByHistoryId(row.ID);
                            return Convertor.ToProductHistory(row, new List<ProductHistoryValue>(), new List<BanknoteSummary>());
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public List<ProductHistory> ListProductHistory(ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistory>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRows();
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return new List<ProductHistory>();
                    }
                }

                string column = GetProductHistoryColumn(sortColumn);
                var cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage);
                //var cmdtxt = String.Format("SELECT * FROM v_products_history t WHERE rownum BETWEEN :rowNum AND :rowNum2 ORDER BY {0} {1}", column,
                //                              sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                            {
                                var values = ListProductHistoryValues(row.ID);
                                var banknotes = ListSummaryBanknotesHistoryId(row.ID);
                                result.Add(Convertor.ToProductHistory(row, values, banknotes));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        private static string GetProductHistoryColumn(ProductHistoryColumns sortColumn)
        {
            string column = String.Empty;
            switch (sortColumn)
            {
                case ProductHistoryColumns.Amount:
                    column = "t.AMOUNT";
                    break;

                case ProductHistoryColumns.CurrencyId:
                    column = "t.CURRENCY_ID";
                    break;

                case ProductHistoryColumns.CurrencyRate:
                    column = "t.rate";
                    break;

                case ProductHistoryColumns.InsertDate:
                    column = "t.INSERT_DATE";
                    break;

                case ProductHistoryColumns.ProductId:
                    column = "t.PRODUCT_ID";
                    break;

                case ProductHistoryColumns.ProductName:
                    column = "t.product_name";
                    break;

                case ProductHistoryColumns.TerminalAddress:
                    column = "t.address";
                    break;

                case ProductHistoryColumns.TerminalId:
                    column = "t.TERMINAL_ID";
                    break;

                case ProductHistoryColumns.TerminalIdentityName:
                    column = "t.identity_name";
                    break;

                case ProductHistoryColumns.TerminalName:
                    column = "t.name";
                    break;

                case ProductHistoryColumns.TransactionId:
                    column = "t.TRANSACTION_ID";
                    break;

                case ProductHistoryColumns.Encashment:
                    column = "t.ENCASHMENT_ID";
                    break;
            }
            return column;
        }

        public Currency GetCurrencies(String id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                using (var adapter = new V_LIST_CURRENCIESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_CURRENCIESDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_LIST_CURRENCIESRow row in table.Rows)
                        {
                            return Convertor.ToCurrency(row);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public List<ProductHistory> ListProductHistory(DateTime from, DateTime to, ProductHistoryColumns sortColumn,
                                                       SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistory>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                to = to.AddDays(1);

                using (var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRowsByDate(from, to);
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return new List<ProductHistory>();
                    }
                }

                string column = GetProductHistoryColumn(sortColumn);

                string cmdtxt =
                    String.Format(
                        "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t WHERE t.insert_date BETWEEN :dateFrom AND :dateTo) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                        column, sortType.ToString(), rowNum, rowNum + perPage);
                Log.Debug(cmdtxt);
                //string cmdtxt =
                //   String.Format(
                //       "SELECT * FROM v_products_history t WHERE insert_date BETWEEN :dateFrom AND :dateTo ORDER BY {0} {1}",
                //       column, sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                    cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                            {
                                var values = ListProductHistoryValues(row.ID);
                                var banknotes = ListSummaryBanknotesHistoryId(row.ID);

                                result.Add(Convertor.ToProductHistory(row, values, banknotes));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<ProductHistory> ListProductHistory(String transactionId, ProductHistoryColumns sortColumn,
                                                       SortType sortType)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistory>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                string column = GetProductHistoryColumn(sortColumn);

                string cmdtxt =
                    String.Format("SELECT * FROM v_products_history t WHERE TRANSACTION_ID = :transId ORDER BY {0} {1}",
                                  column, sortType.ToString());
                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    cmd.Parameters.Add("transId", OracleDbType.Varchar2, ParameterDirection.Input).Value = transactionId;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                            {
                                var values = ListProductHistoryValues(row.ID);
                                var banknotes = ListSummaryBanknotesHistoryId(row.ID);
                                result.Add(Convertor.ToProductHistory(row, values, banknotes));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<ProductHistory> ListProductHistory(DateTime from, DateTime to, int terminalId, ProductHistoryColumns sortColumn,
                                                       SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistory>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                to = to.AddDays(1);

                using (var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRowsTerminalID(from, to, terminalId);
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return new List<ProductHistory>();
                    }
                }

                string column = GetProductHistoryColumn(sortColumn);

                string cmdtxt =
                    String.Format(
                        "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t WHERE t.insert_date BETWEEN :dateFrom AND :dateTo AND t.TERMINAL_ID = :terminalId) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                        column, sortType.ToString(), rowNum, rowNum + perPage);

                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                    cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                    cmd.Parameters.Add("terminalId", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                            {
                                var values = ListProductHistoryValues(row.ID);
                                var banknotes = ListSummaryBanknotesHistoryId(row.ID);

                                result.Add(Convertor.ToProductHistory(row, values, banknotes));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<ProductHistory> ListProductHistoryByProduct(DateTime from, DateTime to, int prodId, ProductHistoryColumns sortColumn,
                                                       SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistory>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                to = to.AddDays(1);

                using (var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRowsProductId(from, to, prodId);
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return new List<ProductHistory>();
                    }
                }

                string column = GetProductHistoryColumn(sortColumn);

                string cmdtxt =
                    String.Format(
                        "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t WHERE t.insert_date BETWEEN :dateFrom AND :dateTo AND t.PRODUCT_ID = :productId) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                        column, sortType.ToString(), rowNum, rowNum + perPage);

                using (var cmd = new OracleCommand(cmdtxt, connection))
                {
                    cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                    cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                    cmd.Parameters.Add("productId", OracleDbType.Int32, ParameterDirection.Input).Value = prodId;

                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                            {
                                var values = ListProductHistoryValues(row.ID);
                                var banknotes = ListSummaryBanknotesHistoryId(row.ID);

                                result.Add(Convertor.ToProductHistory(row, values, banknotes));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<ProductHistory> ListProductHistoryByEncashmentId(DateTime from, DateTime to, int encashmentId, ProductHistoryColumns sortColumn,
                                                       SortType sortType, int rowNum, int perPage, out int count)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistory>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();
                to = to.AddDays(1);

                using (var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = connection })
                {
                    var rawCount = adapter1.CountRowsByEncashmentId(from, to, encashmentId);
                    if (rawCount != null)
                    {
                        count = Convert.ToInt32(rawCount);
                    }
                    else
                    {
                        count = 0;
                        return new List<ProductHistory>();
                    }
                }

                string column = GetProductHistoryColumn(sortColumn);

                using (var cmd = new OracleCommand { Connection = connection })
                {
                    string cmdtxt;
                    if (encashmentId > 0)
                    {
                        cmdtxt = String.Format(
                            "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t WHERE t.insert_date BETWEEN :dateFrom AND :dateTo AND t.ENCASHMENT_ID = :encashmentId) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                            column, sortType.ToString(), rowNum, rowNum + perPage);


                        cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                        cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                        cmd.Parameters.Add("encashmentId", OracleDbType.Int32, ParameterDirection.Input).Value =
                            encashmentId;
                    }
                    else
                    {
                        cmdtxt = String.Format(
                            "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t WHERE t.insert_date BETWEEN :dateFrom AND :dateTo AND t.ENCASHMENT_ID IS NULL) WHERE rn BETWEEN {2} and {3} ORDER BY rn",
                            column, sortType.ToString(), rowNum, rowNum + perPage);


                        cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
                        cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
                    }
                    cmd.CommandText = cmdtxt;


                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
                            {
                                var values = ListProductHistoryValues(row.ID);
                                var banknotes = ListSummaryBanknotesHistoryId(row.ID);

                                result.Add(Convertor.ToProductHistory(row, values, banknotes));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<ProductHistoryValue> ListProductHistoryValues(decimal historyId)
        {
            OracleConnection connection = null;
            var result = new List<ProductHistoryValue>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_PRODUCTS_HISTORY_VALUESTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_PRODUCTS_HISTORY_VALUESDataTable())
                    {
                        adapter.FillByHistoryId(table, historyId);

                        foreach (ds.V_PRODUCTS_HISTORY_VALUESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToProductHistoryValue(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Banknote> ListBanknotesByTerminalId(int terminalId)
        {
            OracleConnection connection = null;
            var result = new List<Banknote>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BANKNOTESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTESDataTable())
                    {
                        adapter.FillByTerminalId(table, terminalId);

                        foreach (ds.V_BANKNOTESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknote(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Banknote> ListBanknotesByTerminalIdNotEncashed(int terminalId)
        {
            OracleConnection connection = null;
            var result = new List<Banknote>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BANKNOTESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTESDataTable())
                    {
                        adapter.FillByTerminalIdNotEncashed(table, terminalId);

                        foreach (ds.V_BANKNOTESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknote(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Banknote> ListBanknotesByEncashmentId(decimal encashmentId)
        {
            OracleConnection connection = null;
            var result = new List<Banknote>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BANKNOTESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTESDataTable())
                    {
                        adapter.FillByEncashmentId(table, encashmentId);

                        foreach (ds.V_BANKNOTESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknote(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Banknote> ListBanknotesByHistoryId(decimal id)
        {
            OracleConnection connection = null;
            var result = new List<Banknote>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BANKNOTESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTESDataTable())
                    {
                        adapter.FillByHistoryId(table, id);

                        foreach (ds.V_BANKNOTESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknote(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<String> ListBanknotesByHistoryId(decimal id, bool val)
        {
            OracleConnection connection = null;
            var result = new List<String>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BANKNOTESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTESDataTable())
                    {
                        adapter.FillByHistoryId(table, id);

                        foreach (ds.V_BANKNOTESRow row in table.Rows)
                        {
                            result.Add(row.AMOUNT.ToString("0"));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Banknote> GetBanknotesByHistoryId(int historyId)
        {
            OracleConnection connection = null;
            var result = new List<Banknote>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BANKNOTESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTESDataTable())
                    {
                        adapter.FillByHistoryId(table, historyId);

                        foreach (ds.V_BANKNOTESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknote(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<CheckField> ListCheckFields(int checkTemplateId)
        {
            OracleConnection connection = null;
            var result = new List<CheckField>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECK_FIELDSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECK_FIELDSDataTable())
                    {
                        adapter.FillByCheckTemplateId(table, checkTemplateId);

                        foreach (ds.V_CHECK_FIELDSRow row in table.Rows)
                        {
                            result.Add(Convertor.ToCheckField(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<CheckField> ListCheckFieldsDigest(int checkTemplateId)
        {
            OracleConnection connection = null;
            var result = new List<CheckField>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECK_FIELDSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECK_FIELDSDataTable())
                    {
                        adapter.FillByCheckTemplateId(table, checkTemplateId);

                        foreach (ds.V_CHECK_FIELDSRow row in table.Rows)
                        {
                            result.Add(Convertor.ToCheckFieldDigest(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public CheckField GetCheckField(int id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECK_FIELDSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECK_FIELDSDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_CHECK_FIELDSRow row in table.Rows)
                        {
                            return Convertor.ToCheckField(row);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public List<CheckFieldType> ListCheckFieldTypes()
        {
            OracleConnection connection = null;
            var result = new List<CheckFieldType>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECK_FIELD_TYPESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECK_FIELD_TYPESDataTable())
                    {
                        adapter.Fill(table);

                        foreach (ds.V_CHECK_FIELD_TYPESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToCheckFieldType(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<CheckType> ListCheckTypes()
        {
            OracleConnection connection = null;
            var result = new List<CheckType>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECK_TYPESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECK_TYPESDataTable())
                    {
                        adapter.Fill(table);
                        foreach (ds.V_CHECK_TYPESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToCheckType(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<CheckTemplate> ListCheckTemplate()
        {
            OracleConnection connection = null;
            var result = new List<CheckTemplate>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECKSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECKSDataTable())
                    {
                        adapter.Fill(table);

                        foreach (ds.V_CHECKSRow row in table.Rows)
                        {
                            var desc = new MultiLanguageString
                                {
                                    ValueAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                                    ValueRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                                    ValueEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN
                                };
                            var type = new CheckType(row.CHECK_TYPE, desc);

                            var fields = ListCheckFields(row.ID);
                            result.Add(Convertor.ToCheckTemplate(row, type, fields));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public CheckTemplate GetCheckTemplate(int id)
        {
            OracleConnection connection = null;
            CheckTemplate result = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CHECKSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CHECKSDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_CHECKSRow row in table.Rows)
                        {
                            var desc = new MultiLanguageString
                                {
                                    ValueAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                                    ValueRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                                    ValueEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN
                                };
                            var type = new CheckType(row.CHECK_TYPE, desc);

                            var fields = ListCheckFields(row.ID);
                            result = Convertor.ToCheckTemplate(row, type, fields);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public int SaveCheckTemplate(CheckTemplate template)
        {
            OracleConnection connection = null;
            object result;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_check(v_id => :v_id, v_check_type => :v_check_type, v_language => :v_language, v_active => :v_active, v_return_id => :v_return_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {

                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = template.Id > 0
                                                                                                         ? (int?)
                                                                                                           template.Id
                                                                                                         : null;
                    cmd.Parameters.Add("v_check_type", OracleDbType.Int32, ParameterDirection.Input).Value =
                        template.CheckType.Id;
                    cmd.Parameters.Add("v_language", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        template.Language;
                    cmd.Parameters.Add("v_active", OracleDbType.Int16, ParameterDirection.Input).Value = template.Active
                                                                                                             ? 1
                                                                                                             : 0;
                    cmd.Parameters.Add("v_return_id", OracleDbType.Int32, ParameterDirection.Output);

                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["v_return_id"].Value;
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result == null ? 0 : ((OracleDecimal)(result)).ToInt32();
        }

        public void SaveCheckField(List<CheckField> fields)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_check_field(v_id => :v_id, v_check_id => :v_check_id, v_image => :v_image, v_value => :v_value, v_field_type => :v_field_type, v_order_number => :v_order_number); end;";

                foreach (var field in fields)
                {
                    using (var cmd = new OracleCommand(cmdText, connection))
                    {

                        cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = field.Id > 0
                                                                                                             ? (int?)
                                                                                                               field.Id
                                                                                                             : null;
                        cmd.Parameters.Add("v_check_id", OracleDbType.Int32, ParameterDirection.Input).Value =
                            field.CheckId;
                        cmd.Parameters.Add("v_image", OracleDbType.Blob, ParameterDirection.Input).Value = field.Image;
                        cmd.Parameters.Add("v_value", OracleDbType.NVarchar2, ParameterDirection.Input).Value =
                            field.Value;
                        cmd.Parameters.Add("v_field_type", OracleDbType.Int32, ParameterDirection.Input).Value =
                            field.FieldType;
                        cmd.Parameters.Add("v_order_number", OracleDbType.Int32, ParameterDirection.Input).Value =
                            field.Order;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void DeleteCheckField(int id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.delete_check_field(v_id => :v_id); end;";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id > 0
                                                                                                         ? (int?)id
                                                                                                         : null;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void DeleteCheckTemplate(int id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.delete_check(v_id => :v_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void ActivateCheckTemplate(int id, bool activateStatus)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.activate_check(v_id => :v_id, v_active => :v_active); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id;
                    cmd.Parameters.Add("v_active", OracleDbType.Int32, ParameterDirection.Input).Value = activateStatus
                                                                                                             ? 1
                                                                                                             : 0;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public List<Branch> ListBranches(BranchColumns sortColumn, SortType sortType)
        {
            OracleConnection connection = null;
            var result = new List<Branch>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var column = String.Empty;
                switch (sortColumn)
                {
                    case BranchColumns.Name:
                        column = "t.NAME";
                        break;

                    case BranchColumns.UserName:
                        column = "t.USERNAME";
                        break;

                    case BranchColumns.UpdateDate:
                        column = "t.UPDATE_DATE";
                        break;

                    case BranchColumns.InsertDate:
                        column = "t.INSERT_DATE";
                        break;

                }

                var cmdText = String.Format("select * from v_branches t ORDER BY {0} {1}", column,
                                            sortType.ToString());

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        using (var table = new ds.V_BRANCHESDataTable())
                        {
                            adapter.Fill(table);

                            foreach (ds.V_BRANCHESRow row in table.Rows)
                            {
                                result.Add(Convertor.ToBranch(row));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public Branch GetBranch(int id)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BRANCHESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BRANCHESDataTable())
                    {
                        adapter.FillById(table, id);

                        foreach (ds.V_BRANCHESRow row in table.Rows)
                        {
                            return Convertor.ToBranch(row);
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return null;
        }

        public void SaveBranch(int id, String name, int userId)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_branch(v_id => :v_id, v_name => :v_name, v_user_id => :v_user_id); end; ";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id > 0
                                                                                                         ? (int?)id
                                                                                                         : null;
                    cmd.Parameters.Add("v_name", OracleDbType.NVarchar2, ParameterDirection.Input).Value = name;
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public List<TerminalStatusCode> ListTerminalStatusCode()
        {
            OracleConnection connection = null;
            var result = new List<TerminalStatusCode>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_TERMINAL_STATUS_CODESTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_TERMINAL_STATUS_CODESDataTable())
                    {
                        adapter.Fill(table);

                        foreach (ds.V_TERMINAL_STATUS_CODESRow row in table.Rows)
                        {
                            result.Add(Convertor.ToTerminalStatusCode(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Branch> ListBranchesByUser(int userId)
        {
            OracleConnection connection = null;
            var result = new List<Branch>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BRANCHES_TO_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BRANCHES_TO_USERSDataTable())
                    {
                        adapter.FillByUserId(table, userId);

                        foreach (ds.V_BRANCHES_TO_USERSRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBranch(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Branch> ListBranchesByUser(decimal userId)
        {
            OracleConnection connection = null;
            var result = new List<Branch>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_BRANCHES_TO_USERSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BRANCHES_TO_USERSDataTable())
                    {
                        adapter.FillByUserId(table, userId);

                        foreach (ds.V_BRANCHES_TO_USERSRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBranch(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<BanknoteSummary> ListSummaryBanknotes(int terminalId)
        {
            OracleConnection connection = null;
            var result = new List<BanknoteSummary>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_BANKNOTES_SUMMARY_BY_TERMTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_BANKNOTES_SUMMARY_BY_TERMDataTable())
                    {
                        adapter.FillByTerminalId(table, terminalId);

                        foreach (ds.V_BANKNOTES_SUMMARY_BY_TERMRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknoteSummary(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<BanknoteSummary> ListSummaryBanknotesHistoryId(int historyId)
        {
            OracleConnection connection = null;
            var result = new List<BanknoteSummary>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_BANKNOTES_SUMMARY_BY_HISTORYTableAdapter
                        {
                            Connection = connection,
                            BindByName = true
                        })
                {
                    using (var table = new ds.V_BANKNOTES_SUMMARY_BY_HISTORYDataTable())
                    {
                        adapter.FillByHistoryId(table, historyId);

                        foreach (ds.V_BANKNOTES_SUMMARY_BY_HISTORYRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknoteSummary(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<BanknoteSummary> ListSummaryBanknotesHistoryId(decimal historyId)
        {
            OracleConnection connection = null;
            var result = new List<BanknoteSummary>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_BANKNOTES_SUMMARY_BY_HISTORYTableAdapter
                        {
                            Connection = connection,
                            BindByName = true
                        })
                {
                    using (var table = new ds.V_BANKNOTES_SUMMARY_BY_HISTORYDataTable())
                    {
                        adapter.FillByHistoryId(table, historyId);

                        foreach (ds.V_BANKNOTES_SUMMARY_BY_HISTORYRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknoteSummary(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<BanknoteSummary> ListSummaryBanknotesEncashmentId(int encashmentId)
        {
            OracleConnection connection = null;
            var result = new List<BanknoteSummary>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_BANKNOTES_SUMMARY_ENCASHMENTTableAdapter
                        {
                            Connection = connection,
                            BindByName = true
                        })
                {
                    using (var table = new ds.V_BANKNOTES_SUMMARY_ENCASHMENTDataTable())
                    {
                        adapter.FillByEncashmentId(table, encashmentId);

                        foreach (ds.V_BANKNOTES_SUMMARY_ENCASHMENTRow row in table.Rows)
                        {
                            result.Add(Convertor.ToBanknoteSummary(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<TerminalType> ListTerminalType()
        {
            OracleConnection connection = null;
            var result = new List<TerminalType>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_TERMINAL_TYPETableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_TERMINAL_TYPEDataTable())
                    {
                        adapter.Fill(table);
                        foreach (ds.V_LIST_TERMINAL_TYPERow row in table.Rows)
                        {
                            result.Add(Convertor.ToTerminalType(row));
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return result;
        }
    }
}
