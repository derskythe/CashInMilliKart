using System;
using System.Collections.Generic;
using System.Data;
using Containers;
using Db.dsTableAdapters;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Db
{
    public partial class OracleDb
    {
        public List<Product> ListProducts()
        {
            CheckConnection();

            var adapter = new V_PRODUCTSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_PRODUCTSDataTable();
            adapter.Fill(table);

            var result = new List<Product>(table.Rows.Count);

            foreach (ds.V_PRODUCTSRow item in table.Rows)
            {
                result.Add(Convertor.ToProduct(item));
            }

            return result;
        }

        public List<CheckTemplate> ListCheckTemplateDigest()
        {
            CheckConnection();

            var adapter = new V_CHECKSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CHECKSDataTable();

            adapter.Fill(table);
            var result = new List<CheckTemplate>();

            foreach (ds.V_CHECKSRow row in table.Rows)
            {
                //var desc = new MultiLanguageString
                //{
                //    ValueAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                //    ValueRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                //    ValueEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN
                //};
                //var type = new CheckType(row.CHECK_TYPE, desc);                
                result.Add(Convertor.ToCheckTemplate(row));
            }

            return result;
        }

        public List<Product> ListProducts(int terminalId)
        {
            CheckConnection();

            var adapter = new V_PRODUCTS_TO_TERMINALSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_PRODUCTS_TO_TERMINALSDataTable();
            adapter.Fill(table);

            var result = new List<Product>(table.Rows.Count);

            foreach (ds.V_PRODUCTS_TO_TERMINALSRow row in table.Rows)
            {
                result.Add(Convertor.ToProduct(row));
            }

            return result;
        }

        public Terminal GetTerminal(int id)
        {
            CheckConnection();

            var adapter = new V_LIST_TERMINALSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_TERMINALSDataTable();
            adapter.FillById(table, id);

            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
            {
                var result = Convertor.ToTerminal(item);
                //Log.Debug(result);
                return result;
            }

            return null;
        }

        public List<Currency> ListCurrencies()
        {
            CheckConnection();

            var adapter = new V_LIST_CURRENCIESTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_CURRENCIESDataTable();
            adapter.Fill(table);

            var result = new List<Currency>(table.Rows.Count);

            foreach (ds.V_LIST_CURRENCIESRow row in table.Rows)
            {
                result.Add(Convertor.ToCurrency(row));
            }

            return result;
        }

        public int GetTerminalStatus(int terminalId)
        {
            CheckConnection();

            var adapter = new V_LIST_TERMINAL_SET_STATUSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_TERMINAL_SET_STATUSDataTable();
            adapter.FillByTerminalId(table, terminalId);

            foreach (ds.V_LIST_TERMINAL_SET_STATUSRow row in table.Rows)
            {
                return Convert.ToInt32(row.STATUS_CODE);
            }

            return 0;
        }

        public void SavePayment(PaymentInfoByProducts info)
        {
            CheckConnection();

            var cmd = _OraCon.CreateCommand();
            cmd.CommandText = "main.save_payment";
            cmd.CommandType = CommandType.StoredProcedure;

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

            transId.OracleDbType = OracleDbType.Int32;
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

        public int GetLastTerminalCommandUserId(int terminalId, int statusCode)
        {
            CheckConnection();

            var adapter = new V_LIST_TERMINAL_SET_STATUSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_LIST_TERMINAL_SET_STATUSDataTable();

            adapter.FillByTerminalIdAndStatusCode(table, terminalId, statusCode);

            foreach (ds.V_LIST_TERMINAL_SET_STATUSRow row in table.Rows)
            {
                return Convert.ToInt32(row.USER_ID);
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
            CheckConnection();

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
            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_status_type", OracleDbType.Int32, ParameterDirection.Input).Value = terminalStatus;
            cmd.Parameters.Add("v_cashcode_status", OracleDbType.Int32, ParameterDirection.Input).Value = cashCodeStatus;
            cmd.Parameters.Add("v_printer_status", OracleDbType.Int32, ParameterDirection.Input).Value = printerStatus;
            cmd.Parameters.Add("v_cashcode_error", OracleDbType.Int32, ParameterDirection.Input).Value = cashcodeError;
            cmd.Parameters.Add("v_cashcode_out_status", OracleDbType.Int32, ParameterDirection.Input).Value = cashcodeOutStatus;
            cmd.Parameters.Add("v_cashcode_suberror", OracleDbType.Int32, ParameterDirection.Input).Value = cashcodeSuberror;
            cmd.Parameters.Add("v_printer_error_state", OracleDbType.Int32, ParameterDirection.Input).Value = printerErrorState;
            cmd.Parameters.Add("v_printer_extended_error_state", OracleDbType.Int32, ParameterDirection.Input).Value = printerExtendedErrorState;
            cmd.Parameters.Add("v_check_count", OracleDbType.Int32, ParameterDirection.Input).Value = checkCount;

            cmd.ExecuteNonQuery();
        }

        public void SaveTerminalStatus(int terminalId, int terminalStatus)
        {
            CheckConnection();

            const string cmdText = "begin main.save_terminal_status(" +
                                   "v_terminal_id => :v_terminal_id, " +
                                   "v_status_type => :v_status_type); " +
                                   "end;";
            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_status_type", OracleDbType.Int32, ParameterDirection.Input).Value = terminalStatus;

            cmd.ExecuteNonQuery();
        }

        public void SaveTerminalKey(int terminalId, byte[] newKey)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_terminal_key(" +
                " v_terminal_id => :v_terminal_id, " +
                " v_new_key => :v_new_key); " +
                "end; ";

            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_new_key", OracleDbType.Blob, ParameterDirection.Input).Value = newKey;

            cmd.ExecuteNonQuery();
        }

        public void SaveTerminalVersion(int terminalId, string version)
        {
            CheckConnection();

            const string cmdText =
                "begin main.save_terminal_version(v_terminal_id => :v_terminal_id, v_version => :v_version); end;";
            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_version", OracleDbType.Varchar2, ParameterDirection.Input).Value = version;

            cmd.ExecuteNonQuery();
        }

        public void TerminalSetDone(int terminalId)
        {
            CheckConnection();

            const string cmdText =
                "begin " +
                "main.terminal_status_done(v_terminal_id => :v_terminal_id); " +
                "end;";

            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int64, ParameterDirection.Input).Value = terminalId;

            cmd.ExecuteNonQuery();
        }

        public void SaveEncashment(Encashment info)
        {
            CheckConnection();

            var cmd = _OraCon.CreateCommand();
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

            if (currencyList.Count > 0 && amount.Count > 0)
            {
                currencies.Value = currencyList.ToArray();
                amounts.Value = amount.ToArray();

                currencies.Size = currencyList.Count;
                amounts.Size = amount.Count;

                cmd.Parameters.Add(currencies);
                cmd.Parameters.Add(amounts);
            }

            cmd.Parameters.Add(terminalId);            

            cmd.ExecuteNonQuery();
        }

        public List<ClientInfo> ListCreditClients(String clientCode)
        {
            CheckConnection();

            var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable();

            adapter.FillByCreditClientCode(table, clientCode);

            var result = new List<ClientInfo>();

            foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
            {
                result.Add(Convertor.ToClientInfo(row));
            }

            return result;
        }

        public List<ClientInfo> ListCreditClients(String creditAccount, String pasportNumber)
        {
            CheckConnection();

            var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable();

            adapter.FillByCreditAccountAndPassport(table, creditAccount, pasportNumber);

            var result = new List<ClientInfo>();

            foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
            {
                result.Add(Convertor.ToClientInfo(row));
            }

            return result;
        }

        public List<ClientInfo> ListClientsByClientAccount(String creditAccount)
        {
            CheckConnection();

            var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable();

            adapter.FillByCreditClientAccount(table, creditAccount);

            var result = new List<ClientInfo>();

            foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
            {
                result.Add(Convertor.ToClientInfo(row));
            }

            return result;
        }

        public List<ClientInfo> ListClientsBolcard(String digits)
        {
            CheckConnection();

            var adapter = new V_CASHIN_BOLCARDSTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CASHIN_BOLCARDSDataTable();

            adapter.FillBy8Digits(table, digits);
            foreach (ds.V_CASHIN_BOLCARDSRow row in table.Rows)
            {
                if (!row.IsCLIENT_ACCOUNTNull())
                {
                    return ListClientsByClientAccount(row.CLIENT_ACCOUNT);
                }
            }

            return new List<ClientInfo>();
        }


        public List<ClientInfo> ListDebitClients(String clientCode)
        {
            CheckConnection();

            var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable();

            adapter.FillByDebitClientCode(table, clientCode);

            var result = new List<ClientInfo>();

            foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
            {
                result.Add(Convertor.ToClientInfo(row));
            }

            return result;
        }

        public List<ClientInfo> ListDebitClients(String creditAccount, String pasportNumber)
        {
            CheckConnection();

            var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_CASHIN_GET_ACCOUNT_INFODataTable();

            adapter.FillByDebitAccountAndPassport(table, creditAccount, pasportNumber);

            var result = new List<ClientInfo>();

            foreach (ds.V_CASHIN_GET_ACCOUNT_INFORow row in table.Rows)
            {
                result.Add(Convertor.ToClientInfo(row));
            }

            return result;
        }

        public void CommitPayment(string cardNumber, float amount, string billSet, int terminalId, int operationType, DateTime timeStamp, String currency)
        {
            CheckConnection();

            const string cmdText = "begin backend.new_credit_payment(v_crd_number => :v_crd_number, v_total_amount => :v_total_amount, v_bill_set => :v_bill_set, v_terminal_id => :v_terminal_id, v_operation_type => :v_operation_type, v_client_timestamp => :v_client_timestamp, v_currency => :v_currency); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_crd_number", OracleDbType.Varchar2, ParameterDirection.Input).Value = cardNumber;
            cmd.Parameters.Add("v_total_amount", OracleDbType.Double, ParameterDirection.Input).Value = amount;
            cmd.Parameters.Add("v_bill_set", OracleDbType.Varchar2, ParameterDirection.Input).Value = billSet;
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_operation_type", OracleDbType.Int32, ParameterDirection.Input).Value = operationType;
            cmd.Parameters.Add("v_client_timestamp", OracleDbType.TimeStamp, ParameterDirection.Input).Value = timeStamp;
            cmd.Parameters.Add("v_currency", OracleDbType.Varchar2, ParameterDirection.Input).Value = currency;

            cmd.ExecuteNonQuery();
        }

        public void RegisterIncassoOrder(int terminalId)
        {
            CheckConnection();

            const string cmdText =
                "begin backend.register_incasso_order(v_terminal_id => :v_terminal_id, v_user_name => :v_user_name); end;";

            var cmd = new OracleCommand(cmdText, _OraCon);
            cmd.Parameters.Add("v_terminal_id", OracleDbType.Int32, ParameterDirection.Input).Value = terminalId;
            cmd.Parameters.Add("v_user_name", OracleDbType.Varchar2, ParameterDirection.Input).Value = String.Empty;

            cmd.ExecuteNonQuery();
        }

        //public List<ClientInfo> ListDebitByClientAccount(String creditAccount)
        //{
        //    CheckConnection();

        //    var adapter = new V_CASHIN_GET_ACCOUNT_INFOTableAdapter { Connection = _OraCon, BindByName = true };
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
            CheckConnection();

            var cmd = _OraCon.CreateCommand();
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

            var result = cmd.Parameters[3].Value;

            return result == null ? 0f : ((Oracle.DataAccess.Types.OracleDecimal)(result)).ToSingle();
        }

        public bool HasTransaction(String transactionId)
        {
            CheckConnection();

            var adapter = new V_PRODUCTS_HISTORYTableAdapter { Connection = _OraCon, BindByName = true };
            var table = new ds.V_PRODUCTS_HISTORYDataTable();

            adapter.FillByTransactionId(table, transactionId);

            foreach (ds.V_PRODUCTS_HISTORYRow row in table.Rows)
            {
                return true;
            }

            return false;
        }
    }
}
