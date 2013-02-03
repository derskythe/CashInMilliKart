using System;
using System.Collections.Generic;
using System.Data;
using Containers;
using Db.dsTableAdapters;
using Oracle.DataAccess.Client;

namespace Db
{
    public partial class OracleDb
    {
        public void StartSession(string sid, int userId)
        {
            CheckConnection();

            const string cmdText = "begin main.start_session(v_sid => :v_sid, v_user_id => :v_user_id); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_sid", OracleDbType.Varchar2, ParameterDirection.Input).Value = sid;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public void UpdateSession(string sid, int userId)
        {
            CheckConnection();

            const string cmdText = "begin main.update_session(v_sid => :v_sid, v_user_id => :v_user_id); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_sid", OracleDbType.Varchar2, ParameterDirection.Input).Value = sid;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public void SaveUser(int? userId, string userName, string password, string salt)
        {
            CheckConnection();

            const string cmdText = "begin main.save_user(v_id => :v_id, v_username => :v_username, v_password => :v_password, v_salt => :v_salt); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
            cmd.Parameters.Add("v_username", OracleDbType.Varchar2, ParameterDirection.Input).Value = userName;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2, ParameterDirection.Input).Value = password;
            cmd.Parameters.Add("v_salt", OracleDbType.Varchar2, ParameterDirection.Input).Value = salt;

            cmd.ExecuteNonQuery();
        }

        public void DeleteUser(int userId)
        {
            CheckConnection();

            const string cmdText = "begin main.delete_user(v_id => :v_id); end;";
            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public void SetTerminalStatusCode(int userId, int terminalId, int status)
        {
            CheckConnection();

            const string cmdText =
                "begin main.set_terminal_status(v_user_id => :v_user_id, v_terminal_id => :v_terminal_id, v_status => :v_status); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_status", OracleDbType.Int32, ParameterDirection.Input).Value = status;

            cmd.ExecuteNonQuery();
        }

        public void SetUserRole(int userId, int roleId)
        {
            CheckConnection();

            const string cmdText = "begin main.set_user_role(v_user_id => :v_user_id, v_role_id => :v_role_id); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
            cmd.Parameters.Add("v_role_id", OracleDbType.Int32, ParameterDirection.Input).Value = roleId;

            cmd.ExecuteNonQuery();
        }

        public void SaveCurrency(string id, string isoName, string name, bool defaultCurrency, int userId)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_currency(v_id => :v_id, v_iso_name => :v_iso_name, v_name => :v_name, v_default_currency => :v_default_currency, v_user_id => :v_user_id); end;";
            var cmd = new OracleCommand(cmdText, _OraCon);


            cmd.Parameters.Add("v_id", OracleDbType.Varchar2, ParameterDirection.Input).Value = id;
            cmd.Parameters.Add("v_iso_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = isoName;
            cmd.Parameters.Add("v_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = name;
            cmd.Parameters.Add("v_default_currency", OracleDbType.Varchar2, ParameterDirection.Input).Value = defaultCurrency;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public void SaveCurrencyRate(string currency, decimal rate, int userId)
        {
            CheckConnection();

            const string cmdText = "begin main.save_currency_rate(v_currency_id => :v_currency_id, v_rate => :v_rate, v_user_id => :v_user_id); end; ";
            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_currency_id", OracleDbType.Varchar2, ParameterDirection.Input).Value = currency;
            cmd.Parameters.Add("v_rate", OracleDbType.Decimal, ParameterDirection.Input).Value = rate;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<ds.V_ROLES_TO_USERSRow> ListRolesToUsersRoleId(int roleId)
        {
            CheckConnection();

            var adapter = new V_ROLES_TO_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_ROLES_TO_USERSDataTable();
            adapter.FillByRoleId(table, roleId);

            foreach (ds.V_ROLES_TO_USERSRow row in table.Rows)
            {
                yield return row;
            }
        }

        public IEnumerable<AccessRole> ListRolesToUsersUserId(decimal userId)
        {
            CheckConnection();

            var adapter = new V_ROLES_TO_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_ROLES_TO_USERSDataTable();
            adapter.FillByUserId(table, userId);

            foreach (ds.V_ROLES_TO_USERSRow row in table.Rows)
            {
                yield return Convertor.ToAccessRole(row);
            }
        }

        public User CheckUser(string username, string password)
        {
            CheckConnection();

            var adapter = new V_LIST_USERSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_USERSDataTable();
            adapter.FillByActive(table, username, password);

            foreach (ds.V_LIST_USERSRow row in table.Rows)
            {
                var roles = ListRolesToUsersUserId(row.ID);
                var user = Convertor.ToUser(row, roles);
                return user;
            }

            return null;
        }

        public User GetUser(int id)
        {
            CheckConnection();

            var adapter = new V_LIST_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_USERSDataTable();
            adapter.FillById(table, id);

            foreach (ds.V_LIST_USERSRow row in table.Rows)
            {
                return Convertor.ToUser(row);
            }

            return null;
        }

        public User GetUser(string username)
        {
            CheckConnection();

            var adapter = new V_LIST_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_USERSDataTable();
            adapter.FillByUsername(table, username);

            foreach (ds.V_LIST_USERSRow row in table.Rows)
            {
                return Convertor.ToUser(row);
            }

            return null;
        }

        public IEnumerable<User> ListUsers()
        {
            CheckConnection();

            var adapter = new V_LIST_USERSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_USERSDataTable();
            adapter.Fill(table);

            foreach (ds.V_LIST_USERSRow row in table.Rows)
            {
                var roles = ListRolesToUsersUserId(row.ID);
                var user = Convertor.ToUser(row, roles);
                yield return user;
            }
        }

        public IEnumerable<AccessRole> ListRoles()
        {
            CheckConnection();

            var adapter = new V_ROLESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_ROLESDataTable();
            adapter.Fill(table);

            foreach (ds.V_ROLESRow row in table.Rows)
            {
                yield return Convertor.ToAccessRole(row);
            }
        }

        public UserSession CheckSession(String sid)
        {
            CheckConnection();

            var adapter = new V_LIST_ACTIVE_SESSIONSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_ACTIVE_SESSIONSDataTable();
            adapter.FillBySid(table, sid);

            foreach (ds.V_LIST_ACTIVE_SESSIONSRow row in table.Rows)
            {
                var roles = ListRolesToUsersUserId(row.USER_ID);
                return Convertor.ToUserSession(row, roles);
            }

            return null;
        }

        public IEnumerable<Terminal> ListTerminals()
        {
            CheckConnection();

            var adapter = new V_LIST_TERMINALSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_TERMINALSDataTable();
            adapter.Fill(table);

            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
            {
                yield return Convertor.ToTerminal(item);
            }
        }

        public IEnumerable<Encashment> ListEncashment()
        {
            CheckConnection();
            var adapter = new V_LIST_ENCASHMENTTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_ENCASHMENTDataTable();
            adapter.Fill(table);

            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
            {
                var fields = ListEncashmentCurrencies(row.ID);
                yield return Convertor.ToEncashment(row, fields);
            }
        }

        public IEnumerable<EncashmentCurrency> ListEncashmentCurrencies(decimal id)
        {
            CheckConnection();
            var adapter = new V_LIST_ENCASHMENT_CURRENCIESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_ENCASHMENT_CURRENCIESDataTable();
            adapter.FillByEncashmentId(table, id);

            foreach (ds.V_LIST_ENCASHMENT_CURRENCIESRow row in table.Rows)
            {
                yield return Convertor.ToEncashmentCurrency(row);
            }
        }
    }
}
