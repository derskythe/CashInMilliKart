using System;
using System.Collections.Generic;
using System.Data;
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

            const string cmdText =
                "begin main.save_user(v_id => :v_id, v_username => :v_username, v_password => :v_password, v_salt => :v_salt); end;";

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

        public int SaveTerminal(int userId, Terminal terminal)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_terminal(v_id => :v_id, v_name => :v_name, v_address => :v_address, v_identity_name => :v_identity_name, v_ip => :v_ip, v_tmp_key => :v_tmp_key, v_user_id => :v_user_id, v_branch_id => :v_branch_id, v_return_id => :v_return_id); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminal.Id > 0
                                                                                                 ? (int?)terminal.Id
                                                                                                 : null;
            cmd.Parameters.Add("v_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = terminal.Name;
            cmd.Parameters.Add("v_address", OracleDbType.Varchar2, ParameterDirection.Input).Value = terminal.Address;
            cmd.Parameters.Add("v_identity_name", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                terminal.IdentityName;
            cmd.Parameters.Add("v_ip", OracleDbType.Varchar2, ParameterDirection.Input).Value = terminal.Ip;
            cmd.Parameters.Add("v_tmp_key", OracleDbType.Blob, ParameterDirection.Input).Value = terminal.TmpKey;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
            cmd.Parameters.Add("v_branch_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminal.BranchId;            
            cmd.Parameters.Add("v_return_id", OracleDbType.Int32, ParameterDirection.Output);

            cmd.ExecuteNonQuery();

            var result = cmd.Parameters["v_return_id"].Value;

            return result == null ? 0 : ((OracleDecimal)(result)).ToInt32();
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
            cmd.Parameters.Add("v_default_currency", OracleDbType.Int16, ParameterDirection.Input).Value =
                defaultCurrency ? 1 : 0;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public void SaveCurrencyRate(string currency, decimal rate, int userId)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_currency_rate(v_currency_id => :v_currency_id, v_rate => :v_rate, v_user_id => :v_user_id); end; ";
            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_currency_id", OracleDbType.Varchar2, ParameterDirection.Input).Value = currency;
            cmd.Parameters.Add("v_rate", OracleDbType.Decimal, ParameterDirection.Input).Value = rate;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;

            cmd.ExecuteNonQuery();
        }

        public ds.V_ROLES_TO_USERSRow[] ListRolesToUsersRoleId(int roleId)
        {
            CheckConnection();

            var adapter = new V_ROLES_TO_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_ROLES_TO_USERSDataTable();
            adapter.FillByRoleId(table, roleId);

            var result = new List<ds.V_ROLES_TO_USERSRow>();
            foreach (ds.V_ROLES_TO_USERSRow row in table.Rows)
            {
                result.Add(row);
            }

            return result.ToArray();
        }

        public List<AccessRole> ListRolesToUsersUserId(decimal userId)
        {
            CheckConnection();

            var adapter = new V_ROLES_TO_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_ROLES_TO_USERSDataTable();
            adapter.FillByUserId(table, userId);

            var result = new List<AccessRole>();
            foreach (ds.V_ROLES_TO_USERSRow row in table.Rows)
            {
                result.Add(Convertor.ToAccessRole(row));
            }

            return result;
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
                var roles = ListRolesToUsersUserId(row.ID);
                return Convertor.ToUser(row, roles);
            }

            return null;
        }

        public User GetUser(decimal id)
        {
            CheckConnection();

            var adapter = new V_LIST_USERSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_USERSDataTable();
            adapter.FillById(table, id);

            foreach (ds.V_LIST_USERSRow row in table.Rows)
            {
                var roles = ListRolesToUsersUserId(row.ID);
                return Convertor.ToUser(row, roles);
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
                var roles = ListRolesToUsersUserId(row.ID);
                return Convertor.ToUser(row, roles);
            }

            return null;
        }

        public List<Currency> ListCurrencies(CurrencyColumns sortColumn, SortType sortType)
        {
            CheckConnection();

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
            var cmd = new OracleCommand(cmdText, _OraCon);
            var adapter = new OracleDataAdapter(cmd);

            var table = new ds.V_LIST_CURRENCIESDataTable();
            adapter.Fill(table);

            var result = new List<Currency>(table.Rows.Count);

            foreach (ds.V_LIST_CURRENCIESRow row in table.Rows)
            {
                result.Add(Convertor.ToCurrency(row));
            }

            return result;
        }

        //public User[] ListUsers()
        //{
        //    CheckConnection();

        //    var adapter = new V_LIST_USERSTableAdapter { Connection = _OraCon, BindByName = true };

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
            CheckConnection();

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
            var cmd = new OracleCommand(cmdText, _OraCon);
            var adapter = new OracleDataAdapter(cmd);
            var table = new ds.V_LIST_USERSDataTable();
            adapter.Fill(table);

            var result = new List<User>();
            foreach (ds.V_LIST_USERSRow row in table.Rows)
            {
                var roles = ListRolesToUsersUserId(row.ID);
                var user = Convertor.ToUser(row, roles);
                result.Add(user);
            }

            return result.ToArray();
        }

        public AccessRole[] ListRoles()
        {
            CheckConnection();

            var adapter = new V_ROLESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_ROLESDataTable();
            adapter.Fill(table);

            var result = new List<AccessRole>();
            foreach (ds.V_ROLESRow row in table.Rows)
            {
                result.Add(Convertor.ToAccessRole(row));
            }

            return result.ToArray();
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

        public Terminal[] ListTerminals(TerminalColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            CheckConnection();

            var adapter1 = new V_LIST_TERMINALSTableAdapter { BindByName = true, Connection = _OraCon };
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

            string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_terminals t) WHERE rn BETWEEN {2} and {3} ORDER BY rn", 
                column, sortType.ToString(), rowNum, rowNum + perPage);
            //string cmdtxt = String.Format("SELECT * FROM v_list_terminals t ORDER BY {0} {1}", column,
            //                              sortType.ToString());
            var cmd = new OracleCommand(cmdtxt, _OraCon);
            //cmd.Parameters.Add("rowNum", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum;
            //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

            var adapter = new OracleDataAdapter(cmd);
            var table = new ds.V_LIST_TERMINALSDataTable();
            adapter.Fill(table);

            var result = new List<Terminal>();
            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
            {
                result.Add(Convertor.ToTerminal(item));
            }

            return result.ToArray();
        }

        public List<Encashment> ListEncashment(EncashmentColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            CheckConnection();

            var adapter1 = new V_LIST_ENCASHMENTTableAdapter { BindByName = true, Connection = _OraCon };
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

            string cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_list_encashment t) WHERE rn BETWEEN {2} and {3} ORDER BY rn", 
                column, sortType.ToString(), rowNum, rowNum + perPage);

            //string cmdtxt = String.Format("SELECT * FROM v_list_encashment t ORDER BY {0} {1}", column,
            //                              sortType.ToString());
            var cmd = new OracleCommand(cmdtxt, _OraCon);
            //cmd.Parameters.Add("rowNum", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum;
            //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

            var adapter = new OracleDataAdapter(cmd);
            var table = new ds.V_LIST_ENCASHMENTDataTable();
            adapter.Fill(table);

            var result = new List<Encashment>();

            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
            {
                var fields = ListEncashmentCurrencies(row.ID);
                var terminal = Convertor.ToTerminal(row);
                var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                result.Add(Convertor.ToEncashment(row, fields, terminal, username));
            }

            return result;
        }

        public Encashment GetEncashment(int id)
        {
            CheckConnection();
            var adapter = new V_LIST_ENCASHMENTTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_ENCASHMENTDataTable();
            adapter.FillById(table, id);

            foreach (ds.V_LIST_ENCASHMENTRow row in table.Rows)
            {
                var fields = ListEncashmentCurrencies(row.ID);
                var terminal = Convertor.ToTerminal(row);
                var username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME;

                return Convertor.ToEncashment(row, fields, terminal, username);
            }

            return null;
        }

        public List<EncashmentCurrency> ListEncashmentCurrencies(decimal id)
        {
            CheckConnection();
            var adapter = new V_LIST_ENCASHMENT_CURRENCIESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_ENCASHMENT_CURRENCIESDataTable();
            adapter.FillByEncashmentId(table, id);
            var result = new List<EncashmentCurrency>();

            foreach (ds.V_LIST_ENCASHMENT_CURRENCIESRow row in table.Rows)
            {
                result.Add(Convertor.ToEncashmentCurrency(row));
            }

            return result;
        }

        public List<ProductHistory> ListProductHistory(ProductHistoryColumns sortColumn, SortType sortType, int rowNum, int perPage, out int count)
        {
            CheckConnection();

            var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = _OraCon };
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

            string column = GetProductHistoryColumn(sortColumn);
            var cmdtxt = String.Format("SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t) WHERE rn BETWEEN {2} and {3} ORDER BY rn", 
                column, sortType.ToString(), rowNum, rowNum + perPage);
            //var cmdtxt = String.Format("SELECT * FROM v_products_history t WHERE rownum BETWEEN :rowNum AND :rowNum2 ORDER BY {0} {1}", column,
            //                              sortType.ToString());
            var cmd = new OracleCommand(cmdtxt, _OraCon);
            //cmd.Parameters.Add("rowNum", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum;
            //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

            var adapter = new OracleDataAdapter(cmd);
            var table = new ds.V_PRODUCTS_HISTORYDataTable();
            adapter.Fill(table);

            var result = new List<ProductHistory>();

            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
            {
                var values = ListProductHistoryValues(row.ID);
                result.Add(Convertor.ToProductHistory(row, values));
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
            }
            return column;
        }

        public Currency GetCurrencies(String id)
        {
            CheckConnection();

            var adapter = new V_LIST_CURRENCIESTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_CURRENCIESDataTable();
            adapter.FillById(table, id);

            //var result = new List<Currency>(table.Rows.Count);

            foreach (ds.V_LIST_CURRENCIESRow row in table.Rows)
            {
                return Convertor.ToCurrency(row);
            }

            return null;
        }

        public List<ProductHistory> ListProductHistory(DateTime from, DateTime to, ProductHistoryColumns sortColumn,
                                                       SortType sortType, int rowNum, int perPage, out int count)
        {
            CheckConnection();

            var adapter1 = new V_PRODUCTS_HISTORYTableAdapter { BindByName = true, Connection = _OraCon };
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

            string column = GetProductHistoryColumn(sortColumn);

            string cmdtxt =
                String.Format(
                    "SELECT * FROM ( SELECT t.*, ROW_NUMBER() OVER (ORDER BY {0} {1}) rn FROM v_products_history t) WHERE insert_date BETWEEN :dateFrom AND :dateTo AND rn BETWEEN {2} and {3} ORDER BY rn",
                    column, sortType.ToString(), rowNum, rowNum + perPage);
            //string cmdtxt =
            //   String.Format(
            //       "SELECT * FROM v_products_history t WHERE insert_date BETWEEN :dateFrom AND :dateTo ORDER BY {0} {1}",
            //       column, sortType.ToString());
            var cmd = new OracleCommand(cmdtxt, _OraCon);
            cmd.Parameters.Add("dateFrom", OracleDbType.Date, ParameterDirection.Input).Value = from;
            cmd.Parameters.Add("dateTo", OracleDbType.Date, ParameterDirection.Input).Value = to;
            //cmd.Parameters.Add("rowNum", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum;
            //cmd.Parameters.Add("rowNum2", OracleDbType.Int32, ParameterDirection.Input).Value = rowNum + perPage;

            var adapter = new OracleDataAdapter(cmd);
            var table = new ds.V_PRODUCTS_HISTORYDataTable();
            adapter.Fill(table);

            var result = new List<ProductHistory>();

            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
            {
                var values = ListProductHistoryValues(row.ID);

                result.Add(Convertor.ToProductHistory(row, values));
            }

            return result;
        }

        public List<ProductHistory> ListProductHistory(String transactionId, ProductHistoryColumns sortColumn,
                                                       SortType sortType)
        {
            CheckConnection();

            string column = GetProductHistoryColumn(sortColumn);

            string cmdtxt =
                String.Format("SELECT * FROM v_products_history t WHERE TRANSACTION_ID = :transId ORDER BY {0} {1}",
                              column, sortType.ToString());
            var cmd = new OracleCommand(cmdtxt, _OraCon);
            cmd.Parameters.Add("transId", OracleDbType.Varchar2, ParameterDirection.Input).Value = transactionId;

            var adapter = new OracleDataAdapter(cmd);
            var table = new ds.V_PRODUCTS_HISTORYDataTable();
            adapter.Fill(table);

            var result = new List<ProductHistory>();

            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
            {
                var values = ListProductHistoryValues(row.ID);
                result.Add(Convertor.ToProductHistory(row, values));
            }

            return result;
        }

        public List<ProductHistoryValue> ListProductHistoryValues(decimal historyId)
        {
            CheckConnection();

            var adapter = new V_PRODUCTS_HISTORY_VALUESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_PRODUCTS_HISTORY_VALUESDataTable();
            adapter.FillByHistoryId(table, historyId);

            var result = new List<ProductHistoryValue>();

            foreach (ds.V_PRODUCTS_HISTORY_VALUESRow row in table.Rows)
            {
                result.Add(Convertor.ToProductHistoryValue(row));
            }

            return result;
        }

        public List<Banknote> GetBanknotesByTerminalId(int terminalId)
        {
            CheckConnection();

            var adapter = new V_BANKNOTESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_BANKNOTESDataTable();

            adapter.FillByTerminalId(table, terminalId);

            var result = new List<Banknote>();

            foreach (ds.V_BANKNOTESRow row in table.Rows)
            {
                result.Add(Convertor.ToBanknote(row));
            }

            return result;
        }

        public List<Banknote> GetBanknotesByEncashmentId(int encashmentId)
        {
            CheckConnection();

            var adapter = new V_BANKNOTESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_BANKNOTESDataTable();

            adapter.FillByEncashmentId(table, encashmentId);

            var result = new List<Banknote>();

            foreach (ds.V_BANKNOTESRow row in table.Rows)
            {
                result.Add(Convertor.ToBanknote(row));
            }

            return result;
        }

        public List<Banknote> GetBanknotesByHistoryId(int historyId)
        {
            CheckConnection();

            var adapter = new V_BANKNOTESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_BANKNOTESDataTable();

            adapter.FillByHistoryId(table, historyId);

            var result = new List<Banknote>();

            foreach (ds.V_BANKNOTESRow row in table.Rows)
            {
                result.Add(Convertor.ToBanknote(row));
            }

            return result;
        }

        public List<CheckField> ListCheckFields(int checkTemplateId)
        {
            CheckConnection();

            var adapter = new V_CHECK_FIELDSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CHECK_FIELDSDataTable();

            adapter.FillByCheckTemplateId(table, checkTemplateId);

            var result = new List<CheckField>();

            foreach (ds.V_CHECK_FIELDSRow row in table.Rows)
            {
                result.Add(Convertor.ToCheckField(row));
            }

            return result;
        }

        public List<CheckFieldType> ListCheckFieldTypes()
        {
            CheckConnection();

            var adapter = new V_CHECK_FIELD_TYPESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CHECK_FIELD_TYPESDataTable();

            adapter.Fill(table);

            var result = new List<CheckFieldType>();
            foreach (ds.V_CHECK_FIELD_TYPESRow row in table.Rows)
            {
                result.Add(Convertor.ToCheckFieldType(row));
            }

            return result;
        }

        public List<CheckType> ListCheckTypes()
        {
            CheckConnection();

            var adapter = new V_CHECK_TYPESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CHECK_TYPESDataTable();

            adapter.Fill(table);

            var result = new List<CheckType>();
            foreach (ds.V_CHECK_TYPESRow row in table.Rows)
            {
                result.Add(Convertor.ToCheckType(row));
            }

            return result;
        }

        public List<CheckTemplate> ListCheckTemplate()
        {
            CheckConnection();

            var adapter = new V_CHECKSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CHECKSDataTable();

            adapter.Fill(table);
            var result = new List<CheckTemplate>();

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

            return result;
        }

        public CheckTemplate GetCheckTemplate(int id)
        {
            CheckConnection();

            var adapter = new V_CHECKSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CHECKSDataTable();

            adapter.FillById(table, id);
            CheckTemplate result = null;

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

            return result;
        }

        public void SaveCheckTemplate(CheckTemplate template)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_check(v_id => :v_id, v_check_type => :v_check_type, v_language => :v_language, v_active => :v_active); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = template.Id > 0 ? (int?)template.Id : null;
            cmd.Parameters.Add("v_check_type", OracleDbType.Int32, ParameterDirection.Input).Value = template.CheckType.Id;
            cmd.Parameters.Add("v_language", OracleDbType.Varchar2, ParameterDirection.Input).Value = template.Language;
            cmd.Parameters.Add("v_active", OracleDbType.Int16, ParameterDirection.Input).Value = template.Active ? 1 : 0;

            cmd.ExecuteNonQuery();
        }

        public void SaveCheckField(List<CheckField> fields)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_check_field(v_id => :v_id, v_check_id => :v_check_id, v_image => :v_image, v_value => :v_value, v_field_type => :v_field_type, v_order_number => :v_order_number); end;";

            foreach (var field in fields)
            {
                var cmd = new OracleCommand(cmdText, _OraCon);

                cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = field.Id > 0
                                                                                                     ? (int?)field.Id
                                                                                                     : null;
                cmd.Parameters.Add("v_check_id", OracleDbType.Int32, ParameterDirection.Input).Value = field.CheckId;
                cmd.Parameters.Add("v_image", OracleDbType.Blob, ParameterDirection.Input).Value = field.Image;
                cmd.Parameters.Add("v_value", OracleDbType.NVarchar2, ParameterDirection.Input).Value = field.Value;
                cmd.Parameters.Add("v_field_type", OracleDbType.Int32, ParameterDirection.Input).Value = field.FieldType;
                cmd.Parameters.Add("v_order_number", OracleDbType.Int32, ParameterDirection.Input).Value = field.Order;

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCheckTemplate(int id)
        {
            CheckConnection();

            const string cmdText =
                "begin main.delete_check(v_id => :v_id); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id;
            cmd.ExecuteNonQuery();
        }

        public void ActivateCheckTemplate(int id, bool activateStatus)
        {
            CheckConnection();

            const string cmdText =
                "begin main.activate_check(v_id => :v_id, v_active => :v_active); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id;
            cmd.Parameters.Add("v_active", OracleDbType.Int32, ParameterDirection.Input).Value = activateStatus ? 1 : 0;
            cmd.ExecuteNonQuery();
        }

        public List<Branch> ListBranches(BranchColumns sortColumn, SortType sortType)
        {
            CheckConnection();

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

            var cmd = new OracleCommand(cmdText, _OraCon);
            var adapter = new OracleDataAdapter(cmd);

            var table = new ds.V_BRANCHESDataTable();

            adapter.Fill(table);
            var result = new List<Branch>();

            foreach (ds.V_BRANCHESRow row in table.Rows)
            {
                result.Add(Convertor.ToBranch(row));
            }

            return result;
        }

        public Branch GetBranch(int id)
        {
            CheckConnection();
            var adapter = new V_BRANCHESTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_BRANCHESDataTable();
            adapter.FillById(table, id);
            
            foreach (ds.V_BRANCHESRow row in table.Rows)
            {
                return Convertor.ToBranch(row);
            }

            return null;
        }

        public void SaveBranch(int id, String name, int userId)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_branch(v_id => :v_id, v_name => :v_name, v_user_id => :v_user_id); end; ";
            var cmd = new OracleCommand(cmdText, _OraCon);

            cmd.Parameters.Add("v_id", OracleDbType.Int32, ParameterDirection.Input).Value = id > 0 ? (int?)id : null;
            cmd.Parameters.Add("v_name", OracleDbType.NVarchar2, ParameterDirection.Input).Value = name;
            cmd.Parameters.Add("v_user_id", OracleDbType.Int32, ParameterDirection.Input).Value = userId;
            cmd.ExecuteNonQuery();
        }
    }
}
