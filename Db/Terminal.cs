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
        public List<Product> ListProducts()
        {
            OracleConnection connection = null;

            var result = new List<Product>();
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();
                var adapter = new V_PRODUCTSTableAdapter { Connection = connection, BindByName = true };

                var table = new ds.V_PRODUCTSDataTable();
                adapter.Fill(table);



                foreach (ds.V_PRODUCTSRow item in table.Rows)
                {
                    result.Add(Convertor.ToProduct(item));
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

        public List<CheckTemplate> ListCheckTemplateDigest()
        {
            OracleConnection connection = null;

            var result = new List<CheckTemplate>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var adapter = new V_CHECKSTableAdapter { Connection = connection, BindByName = true };
                var table = new ds.V_CHECKSDataTable();

                adapter.Fill(table);


                foreach (ds.V_CHECKSRow row in table.Rows)
                {
                    //var desc = new MultiLanguageString
                    //{
                    //    ValueAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                    //    ValueRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                    //    ValueEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN
                    //};
                    //var type = new CheckType(row.CHECK_TYPE, desc);     
                    var fields = ListCheckFieldsDigest(row.ID);
                    result.Add(Convertor.ToCheckTemplate(row, fields));
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

        public List<Product> ListProducts(int terminalId)
        {
            OracleConnection connection = null;
            var result = new List<Product>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var adapter = new V_PRODUCTS_TO_TERMINALSTableAdapter { Connection = connection, BindByName = true };
                var table = new ds.V_PRODUCTS_TO_TERMINALSDataTable();
                adapter.Fill(table);



                foreach (ds.V_PRODUCTS_TO_TERMINALSRow row in table.Rows)
                {
                    result.Add(Convertor.ToProduct(row));
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

        public Terminal GetTerminal(int id)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var adapter = new V_LIST_TERMINALSTableAdapter { Connection = connection, BindByName = true };

                var table = new ds.V_LIST_TERMINALSDataTable();
                adapter.FillById(table, id);

                foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
                {
                    var result = Convertor.ToTerminal(item);
                    //Log.Debug(result);
                    return result;
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

        public List<Currency> ListCurrencies()
        {
            OracleConnection connection = null;
            var result = new List<Currency>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();


                using (var adapter = new V_LIST_CURRENCIESTableAdapter { Connection = connection, BindByName = true })
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

        public int GetTerminalStatus(int terminalId)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_TERMINAL_SET_STATUSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_TERMINAL_SET_STATUSDataTable())
                    {
                        adapter.FillByTerminalId(table, terminalId);

                        foreach (ds.V_LIST_TERMINAL_SET_STATUSRow row in table.Rows)
                        {
                            return Convert.ToInt32(row.STATUS_CODE);
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
            return 0;
        }

        [Obsolete]
        public void SavePayment(PaymentInfoByProducts info)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var transId = new OracleParameter();
                var terminalId = new OracleParameter();
                var productId = new OracleParameter();
                var currencyId = new OracleParameter();
                var currencyRateId = new OracleParameter();
                var amount = new OracleParameter();
                var terminalDate = new OracleParameter();
                var creditNumber = new OracleParameter();
                var type = new OracleParameter();
                var banknotes = new OracleParameter();
                var values = new OracleParameter();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "main.save_payment";
                    cmd.CommandType = CommandType.StoredProcedure;

                    transId.OracleDbType = OracleDbType.Varchar2;
                    terminalId.OracleDbType = OracleDbType.Int32;
                    productId.OracleDbType = OracleDbType.Int32;
                    currencyId.OracleDbType = OracleDbType.Varchar2;
                    currencyRateId.OracleDbType = OracleDbType.Decimal;
                    amount.OracleDbType = OracleDbType.Int32;
                    creditNumber.OracleDbType = OracleDbType.Varchar2;
                    type.OracleDbType = OracleDbType.Int32;
                    terminalDate.OracleDbType = OracleDbType.TimeStamp;
                    banknotes.OracleDbType = OracleDbType.Int32;
                    values.OracleDbType = OracleDbType.Varchar2;

                    banknotes.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    values.CollectionType = OracleCollectionType.PLSQLAssociativeArray;

                    transId.Value = info.TransactionId;
                    terminalId.Value = info.TerminalId;
                    productId.Value = info.ProductId;
                    currencyId.Value = info.Currency;
                    currencyRateId.Value = info.CurrencyRate;
                    amount.Value = info.Amount;
                    terminalDate.Value = info.TerminalDate;
                    banknotes.Value = info.Banknotes;
                    values.Value = info.Values;
                    creditNumber.Value = info.CreditNumber;
                    type.Value = info.OperationType;

                    banknotes.Size = info.Banknotes.Length;
                    values.Size = info.Values.Length;

                    cmd.Parameters.Add(transId);
                    cmd.Parameters.Add(terminalId);
                    cmd.Parameters.Add(productId);
                    cmd.Parameters.Add(currencyId);
                    cmd.Parameters.Add(currencyRateId);
                    cmd.Parameters.Add(amount);
                    cmd.Parameters.Add(terminalDate);
                    cmd.Parameters.Add(creditNumber);
                    cmd.Parameters.Add(type);
                    cmd.Parameters.Add(banknotes);
                    cmd.Parameters.Add(values);

                    cmd.ExecuteNonQuery();
                }

                transId.Dispose();
                terminalId.Dispose();
                productId.Dispose();
                currencyId.Dispose();
                currencyRateId.Dispose();
                amount.Dispose();
                terminalDate.Dispose();
                creditNumber.Dispose();
                type.Dispose();
                banknotes.Dispose();
                values.Dispose();
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

        public void SavePayment(TerminalPaymentInfo info)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var transId = new OracleParameter();
                var terminalId = new OracleParameter();
                var productId = new OracleParameter();
                var currencyId = new OracleParameter();
                var currencyRateId = new OracleParameter();
                var amount = new OracleParameter();
                var terminalDate = new OracleParameter();
                var creditNumber = new OracleParameter();
                var type = new OracleParameter();
                var banknotes = new OracleParameter();
                var values = new OracleParameter();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "main.save_payment";
                    cmd.CommandType = CommandType.StoredProcedure;

                    transId.OracleDbType = OracleDbType.Varchar2;
                    terminalId.OracleDbType = OracleDbType.Int32;
                    productId.OracleDbType = OracleDbType.Int32;
                    currencyId.OracleDbType = OracleDbType.Varchar2;
                    currencyRateId.OracleDbType = OracleDbType.Decimal;
                    amount.OracleDbType = OracleDbType.Int32;
                    creditNumber.OracleDbType = OracleDbType.Varchar2;
                    type.OracleDbType = OracleDbType.Int32;
                    terminalDate.OracleDbType = OracleDbType.TimeStamp;
                    banknotes.OracleDbType = OracleDbType.Int32;
                    values.OracleDbType = OracleDbType.Varchar2;

                    banknotes.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    values.CollectionType = OracleCollectionType.PLSQLAssociativeArray;

                    transId.Value = info.TransactionId;
                    terminalId.Value = info.TerminalId;
                    productId.Value = info.ProductId;
                    currencyId.Value = info.Currency;
                    currencyRateId.Value = info.CurrencyRate;
                    amount.Value = info.Amount;
                    terminalDate.Value = info.TerminalDate;
                    banknotes.Value = info.Banknotes;
                    values.Value = info.Values;
                    creditNumber.Value = info.CreditNumber;
                    type.Value = info.OperationType;

                    banknotes.Size = info.Banknotes.Length;
                    values.Size = info.Values.Length;

                    cmd.Parameters.Add(transId);
                    cmd.Parameters.Add(terminalId);
                    cmd.Parameters.Add(productId);
                    cmd.Parameters.Add(currencyId);
                    cmd.Parameters.Add(currencyRateId);
                    cmd.Parameters.Add(amount);
                    cmd.Parameters.Add(terminalDate);
                    cmd.Parameters.Add(creditNumber);
                    cmd.Parameters.Add(type);
                    cmd.Parameters.Add(banknotes);
                    cmd.Parameters.Add(values);

                    cmd.ExecuteNonQuery();
                }

                transId.Dispose();
                terminalId.Dispose();
                productId.Dispose();
                currencyId.Dispose();
                currencyRateId.Dispose();
                amount.Dispose();
                terminalDate.Dispose();
                creditNumber.Dispose();
                type.Dispose();
                banknotes.Dispose();
                values.Dispose();
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

        public void SavePaymentWithBackend(TerminalPaymentInfo info)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                var transId = new OracleParameter();
                var terminalId = new OracleParameter();
                var productId = new OracleParameter();
                var currencyId = new OracleParameter();
                var currencyRateId = new OracleParameter();
                var amount = new OracleParameter();
                var terminalDate = new OracleParameter();
                var creditNumber = new OracleParameter();
                var type = new OracleParameter();
                var banknotes = new OracleParameter();
                var paramBills = new OracleParameter();
                var values = new OracleParameter();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "main.save_payment_with_backend";
                    cmd.CommandType = CommandType.StoredProcedure;
                    string bills = String.Join(";", info.Banknotes);

                    transId.OracleDbType = OracleDbType.Varchar2;
                    terminalId.OracleDbType = OracleDbType.Int32;
                    productId.OracleDbType = OracleDbType.Int32;
                    currencyId.OracleDbType = OracleDbType.Varchar2;
                    currencyRateId.OracleDbType = OracleDbType.Decimal;
                    amount.OracleDbType = OracleDbType.Int32;
                    creditNumber.OracleDbType = OracleDbType.Varchar2;
                    type.OracleDbType = OracleDbType.Int32;
                    terminalDate.OracleDbType = OracleDbType.TimeStamp;
                    paramBills.OracleDbType = OracleDbType.Varchar2;
                    banknotes.OracleDbType = OracleDbType.Int32;
                    values.OracleDbType = OracleDbType.Varchar2;

                    banknotes.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    values.CollectionType = OracleCollectionType.PLSQLAssociativeArray;

                    transId.Value = info.TransactionId;
                    terminalId.Value = info.TerminalId;
                    productId.Value = info.ProductId;
                    currencyId.Value = info.Currency;
                    currencyRateId.Value = info.CurrencyRate;
                    amount.Value = info.Amount;
                    terminalDate.Value = info.TerminalDate;
                    banknotes.Value = info.Banknotes;
                    values.Value = info.Values;
                    creditNumber.Value = info.CreditNumber;
                    type.Value = info.OperationType;
                    paramBills.Value = bills;

                    banknotes.Size = info.Banknotes.Length;
                    values.Size = info.Values.Length;

                    cmd.Parameters.Add(transId);
                    cmd.Parameters.Add(terminalId);
                    cmd.Parameters.Add(productId);
                    cmd.Parameters.Add(currencyId);
                    cmd.Parameters.Add(currencyRateId);
                    cmd.Parameters.Add(amount);
                    cmd.Parameters.Add(terminalDate);
                    cmd.Parameters.Add(creditNumber);
                    cmd.Parameters.Add(type);
                    cmd.Parameters.Add(paramBills);
                    cmd.Parameters.Add(banknotes);
                    cmd.Parameters.Add(values);

                    cmd.ExecuteNonQuery();
                }

                transId.Dispose();
                terminalId.Dispose();
                productId.Dispose();
                currencyId.Dispose();
                currencyRateId.Dispose();
                amount.Dispose();
                terminalDate.Dispose();
                banknotes.Dispose();
                values.Dispose();
                creditNumber.Dispose();
                type.Dispose();
                paramBills.Dispose();
                banknotes.Dispose();
                values.Dispose();
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

        public int GetLastTerminalCommandUserId(int terminalId, int statusCode)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_LIST_TERMINAL_SET_STATUSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_LIST_TERMINAL_SET_STATUSDataTable())
                    {
                        adapter.FillByTerminalIdAndStatusCode(table, terminalId, statusCode);

                        foreach (ds.V_LIST_TERMINAL_SET_STATUSRow row in table.Rows)
                        {
                            return Convert.ToInt32(row.USER_ID);
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

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalId"></param>
        /// <param name="terminalStatus"></param>
        /// <param name="cashCodeStatus"></param>
        /// <param name="printerStatus"></param>
        /// <param name="cashcodeError"></param>
        /// <param name="cashcodeOutStatus"></param>
        /// <param name="cashcodeSuberror"></param>
        /// <param name="printerErrorState"></param>
        /// <param name="printerExtendedErrorState"></param>
        /// <param name="checkCount"></param>
        public void SaveTerminalStatus(int terminalId, int terminalStatus, int cashCodeStatus, int printerStatus, int cashcodeError, int cashcodeOutStatus, int cashcodeSuberror, int printerErrorState, int printerExtendedErrorState, int checkCount)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText = "begin main.save_terminal_status(" +
                                       "v_terminal_id => :v_terminal_id, " +
                                       "v_status_type => :v_status_type, " +
                                       "v_cashcode_status => :v_cashcode_status, " +
                                       "v_printer_status => :v_printer_status, " +
                                       "v_cashcode_error => :v_cashcode_error, " +
                                       "v_cashcode_out_status => :v_cashcode_out_status, " +
                                       "v_cashcode_suberror => :v_cashcode_suberror, " +
                                       "v_printer_error_state => :v_printer_error_state, " +
                                       "v_printer_extended_error_state => :v_printer_extended_error_state," +
                                       "v_check_count => :v_check_count); " +
                                       "end; ";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_status_type", OracleDbType.Int32, ParameterDirection.Input).Value = terminalStatus;
                    cmd.Parameters.Add("v_cashcode_status", OracleDbType.Int32, ParameterDirection.Input).Value =
                        cashCodeStatus;
                    cmd.Parameters.Add("v_printer_status", OracleDbType.Int32, ParameterDirection.Input).Value =
                        printerStatus;
                    cmd.Parameters.Add("v_cashcode_error", OracleDbType.Int32, ParameterDirection.Input).Value =
                        cashcodeError;
                    cmd.Parameters.Add("v_cashcode_out_status", OracleDbType.Int32, ParameterDirection.Input).Value =
                        cashcodeOutStatus;
                    cmd.Parameters.Add("v_cashcode_suberror", OracleDbType.Int32, ParameterDirection.Input).Value =
                        cashcodeSuberror;
                    cmd.Parameters.Add("v_printer_error_state", OracleDbType.Int32, ParameterDirection.Input).Value =
                        printerErrorState;
                    cmd.Parameters.Add("v_printer_extended_error_state", OracleDbType.Int32, ParameterDirection.Input).Value
                        = printerExtendedErrorState;
                    cmd.Parameters.Add("v_check_count", OracleDbType.Int32, ParameterDirection.Input).Value = checkCount;

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

        public void SaveTerminalStatus(int terminalId, int terminalStatus)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText = "begin main.save_terminal_status(" +
                                       "v_terminal_id => :v_terminal_id, " +
                                       "v_status_type => :v_status_type); " +
                                       "end;";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_status_type", OracleDbType.Int32, ParameterDirection.Input).Value = terminalStatus;

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

        public void SaveTerminalKey(int terminalId, byte[] newKey)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_terminal_key(" +
                    " v_terminal_id => :v_terminal_id, " +
                    " v_new_key => :v_new_key); " +
                    "end; ";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_new_key", OracleDbType.Blob, ParameterDirection.Input).Value = newKey;

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

        public void SaveTerminalVersion(int terminalId, string version)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_terminal_version(v_terminal_id => :v_terminal_id, v_version => :v_version); end;";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_version", OracleDbType.Varchar2, ParameterDirection.Input).Value = version;

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

        public void SaveTerminalVersionExt(int terminalId, string version, string cashCodeVersion, string billsTable)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin main.save_terminal_version(v_terminal_id => :v_terminal_id, v_version => :v_version, v_cashcode_version => :v_cashcode_version, v_bills_table => :v_bills_table); end;";
                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_version", OracleDbType.Varchar2, ParameterDirection.Input).Value = version;
                    cmd.Parameters.Add("v_cashcode_version", OracleDbType.Varchar2, ParameterDirection.Input).Value = cashCodeVersion;
                    cmd.Parameters.Add("v_bills_table", OracleDbType.Varchar2, ParameterDirection.Input).Value = billsTable;

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

        public void TerminalSetDone(int terminalId)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin " +
                    "main.terminal_status_done(v_terminal_id => :v_terminal_id); " +
                    "end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
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

        public void SaveEncashment(Encashment info)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "main.save_encashment";
                    cmd.CommandType = CommandType.StoredProcedure;

                    var terminalId = new OracleParameter();
                    var currencies = new OracleParameter();
                    var amounts = new OracleParameter();

                    terminalId.OracleDbType = OracleDbType.Int32;
                    currencies.OracleDbType = OracleDbType.Varchar2;
                    amounts.OracleDbType = OracleDbType.Int32;

                    currencies.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    amounts.CollectionType = OracleCollectionType.PLSQLAssociativeArray;

                    terminalId.Value = info.TerminalId;
                    var amount = new List<int>();
                    var currencyList = new List<string>();

                    foreach (var currency in info.Currencies)
                    {
                        amount.Add(currency.Amount);
                        currencyList.Add(currency.Currency);
                    }

                    cmd.Parameters.Add(terminalId);

                    if (currencyList.Count > 0 && amount.Count > 0)
                    {
                        currencies.Value = currencyList.ToArray();
                        amounts.Value = amount.ToArray();

                        currencies.Size = currencyList.Count;
                        amounts.Size = amount.Count;

                        cmd.Parameters.Add(currencies);
                        cmd.Parameters.Add(amounts);
                    }

                    cmd.ExecuteNonQuery();

                    terminalId.Dispose();
                    currencies.Dispose();
                    amounts.Dispose();
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

        public List<ClientInfo> ListCreditClients(String clientCode)
        {
            OracleConnection connection = null;
            var result = new List<ClientInfo>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable())
                    {
                        adapter.FillByCreditClientCode(table, clientCode);

                        foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
                        {
                            result.Add(Convertor.ToClientInfo(row));
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

        public List<ClientInfo> ListCreditClients(String creditAccount, String pasportNumber)
        {
            OracleConnection connection = null;
            var result = new List<ClientInfo>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable())
                    {
                        adapter.FillByCreditAccountAndPassport(table, creditAccount, pasportNumber);

                        foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
                        {
                            result.Add(Convertor.ToClientInfo(row));
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

        public List<ClientInfo> ListClientsByClientAccount(String creditAccount)
        {
            OracleConnection connection = null;
            var result = new List<ClientInfo>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable())
                    {
                        adapter.FillByCreditClientAccount(table, creditAccount);

                        foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
                        {
                            result.Add(Convertor.ToClientInfo(row));
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

        public List<ClientInfo> ListClientsBolcard(String digits)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CASHIN_BOLCARDSTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CASHIN_BOLCARDSDataTable())
                    {
                        adapter.FillBy8Digits(table, digits);
                        foreach (ds.V_CASHIN_BOLCARDSRow row in table.Rows)
                        {
                            if (!row.IsCLIENT_ACCOUNTNull())
                            {
                                return ListClientsByClientAccount(row.CLIENT_ACCOUNT);
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

            return new List<ClientInfo>();
        }

        public List<ClientInfo> ListClientsBolcardExt(String digits)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_CASHIN_BOLCARDS_EXTTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_CASHIN_BOLCARDS_EXTDataTable())
                    {
                        adapter.FillByPan(table, digits);
                        foreach (ds.V_CASHIN_BOLCARDS_EXTRow row in table.Rows)
                        {
                            if (!row.IsCLIENT_ACCOUNTNull())
                            {
                                return ListClientsByClientAccount(row.CLIENT_ACCOUNT);
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

            return new List<ClientInfo>();
        }

        public List<ClientInfo> ListDebitClients(String clientCode)
        {
            OracleConnection connection = null;
            var result = new List<ClientInfo>();

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable())
                    {
                        adapter.FillByDebitClientCode(table, clientCode);

                        foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
                        {
                            result.Add(Convertor.ToClientInfo(row));
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

        public List<ClientInfo> ListDebitClients(String creditAccount, String pasportNumber)
        {
            OracleConnection connection = null;
            var result = new List<ClientInfo>();
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (
                    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = connection, BindByName = true }
                    )
                {
                    using (var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable())
                    {
                        adapter.FillByDebitAccountAndPassport(table, creditAccount, pasportNumber);

                        foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
                        {
                            result.Add(Convertor.ToClientInfo(row));
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

        public void CommitPayment(string cardNumber, float amount, string billSet, int terminalId, int operationType, DateTime timeStamp, String currency)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText = "begin backend.new_credit_payment(v_crd_number => :v_crd_number, v_total_amount => :v_total_amount, v_bill_set => :v_bill_set, v_terminal_id => :v_terminal_id, v_operation_type => :v_operation_type, v_client_timestamp => :v_client_timestamp, v_currency => :v_currency); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_crd_number", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        cardNumber;
                    cmd.Parameters.Add("v_total_amount", OracleDbType.Double, ParameterDirection.Input).Value = amount;
                    cmd.Parameters.Add("v_bill_set", OracleDbType.Varchar2, ParameterDirection.Input).Value = billSet;
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_operation_type", OracleDbType.Int32, ParameterDirection.Input).Value =
                        operationType;
                    cmd.Parameters.Add("v_client_timestamp", OracleDbType.TimeStamp, ParameterDirection.Input).Value =
                        timeStamp;
                    cmd.Parameters.Add("v_currency", OracleDbType.Varchar2, ParameterDirection.Input).Value = currency;

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

        public void CommitPaymentManual(string cardNumber, float amount, string billSet, int terminalId, int operationType, DateTime timeStamp, String currency, int billGroup)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText = "begin new_credit_payment_manual(v_crd_number => :v_crd_number, v_total_amount => :v_total_amount, v_bill_set => :v_bill_set, v_terminal_id => :v_terminal_id, v_operation_type => :v_operation_type, v_client_timestamp => :v_client_timestamp, v_currency => :v_currency, v_billgrp_id => :v_billgrp_id); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_crd_number", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        cardNumber;
                    cmd.Parameters.Add("v_total_amount", OracleDbType.Double, ParameterDirection.Input).Value = amount;
                    cmd.Parameters.Add("v_bill_set", OracleDbType.Varchar2, ParameterDirection.Input).Value = billSet;
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_operation_type", OracleDbType.Int32, ParameterDirection.Input).Value =
                        operationType;
                    cmd.Parameters.Add("v_client_timestamp", OracleDbType.TimeStamp, ParameterDirection.Input).Value =
                        timeStamp;
                    cmd.Parameters.Add("v_currency", OracleDbType.Varchar2, ParameterDirection.Input).Value = currency;
                    cmd.Parameters.Add("v_billgrp_id", OracleDbType.Int32, ParameterDirection.Input).Value = billGroup;

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

        public void RegisterIncassoOrder(int terminalId)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                const string cmdText =
                    "begin backend.register_incasso_order(v_terminal_id => :v_terminal_id, v_user_name => :v_user_name); end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
                    cmd.Parameters.Add("v_user_name", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        String.Empty;

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

        //public List<ClientInfo> ListDebitByClientAccount(String creditAccount)
        //{
        //    CheckConnection();

        //    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = connection, BindByName = true };
        //    var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable();

        //    adapter.FillByDebitClientAccount(table, creditAccount);

        //    var result = new List<ClientInfo>();

        //    foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
        //    {
        //        result.Add(Convertor.ToClientInfo(row));
        //    }

        //    return result;
        //}
        public float GetBonusAmount(String creditNumber, float amount, String currency)
        {
            OracleConnection connection = null;
            object result;
            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "backend.get_bonus_amount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    var returnValue = new OracleParameter();
                    var paramCreditNumber = new OracleParameter();
                    var paramCurrency = new OracleParameter();
                    var paramAmount = new OracleParameter();

                    paramCreditNumber.OracleDbType = OracleDbType.NVarchar2;
                    paramCurrency.OracleDbType = OracleDbType.NVarchar2;
                    paramAmount.OracleDbType = OracleDbType.Double;
                    returnValue.OracleDbType = OracleDbType.Double;

                    paramCreditNumber.Direction = ParameterDirection.Input;
                    paramCurrency.Direction = ParameterDirection.Input;
                    paramAmount.Direction = ParameterDirection.Input;
                    returnValue.Direction = ParameterDirection.Output;

                    paramCreditNumber.Value = creditNumber;
                    paramCurrency.Value = currency;
                    paramAmount.Value = amount;

                    cmd.Parameters.Add(paramCreditNumber);
                    cmd.Parameters.Add(paramCurrency);
                    cmd.Parameters.Add(paramAmount);
                    cmd.Parameters.Add(returnValue);

                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters[3].Value;

                    returnValue.Dispose();
                    paramCreditNumber.Dispose();
                    paramCurrency.Dispose();
                    paramAmount.Dispose();
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
            return result == null ? 0f : ((Oracle.DataAccess.Types.OracleDecimal)(result)).ToSingle();

        }

        public bool HasTransaction(String transactionId)
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString); connection.Open();

                using (var adapter = new V_PRODUCTS_HISTORYTableAdapter { Connection = connection, BindByName = true })
                {
                    using (var table = new ds.V_PRODUCTS_HISTORYDataTable())
                    {
                        adapter.FillByTransactionId(table, transactionId);

                        if (table.Rows.Count > 0)
                        {
                            return true;
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

            return false;
        }

        public void SaveRequest(String phoneNumber)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText =
                    "begin   " +
                    "OCP_REQUEST.save_request(" +
                    "v_phone => :v_phone); " +
                    "end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_phone", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                        phoneNumber;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Close();
                }
            }
        }

        public void UpdateRequest(long id, int status)
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                const string cmdText =
                    "begin " +
                    "OCP_REQUEST.update_request_status(" +
                    "v_id => :v_id, " +
                    "v_status => :v_status); " +
                    "end;";

                using (var cmd = new OracleCommand(cmdText, connection))
                {
                    cmd.Parameters.Add("v_id", OracleDbType.Int64, ParameterDirection.Input).Value =
                        id;
                    cmd.Parameters.Add("v_status", OracleDbType.Int16, ParameterDirection.Input).Value =
                        status;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Close();
                }
            }
        }

        public IEnumerable<ds.V_ACTIVE_REQUESTRow> ListActiveRequest()
        {
            OracleConnection connection = null;

            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();
                using (var adapter = new V_ACTIVE_REQUESTTableAdapter { BindByName = true, Connection = connection })
                {
                    using (var table = new ds.V_ACTIVE_REQUESTDataTable())
                    {
                        adapter.Fill(table);

                        foreach (ds.V_ACTIVE_REQUESTRow row in table.Rows)
                        {
                            yield return row;
                        }
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Close();
                }
            }
        }
    }
}
