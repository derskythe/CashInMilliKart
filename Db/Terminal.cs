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
            var banknotes = new OracleParameter();
            var values = new OracleParameter();

            transId.OracleDbType = OracleDbType.Int32;
            terminalId.OracleDbType = OracleDbType.Int32;
            productId.OracleDbType = OracleDbType.Int32;
            currencyId.OracleDbType = OracleDbType.Varchar2;
            currencyRateId.OracleDbType = OracleDbType.Decimal;
            amount.OracleDbType = OracleDbType.Int32;
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

            banknotes.Size = info.Banknotes.Length;
            values.Size = info.Values.Length;

            cmd.Parameters.Add(transId);
            cmd.Parameters.Add(terminalId);
            cmd.Parameters.Add(productId);
            cmd.Parameters.Add(currencyId);
            cmd.Parameters.Add(currencyRateId);
            cmd.Parameters.Add(amount);
            cmd.Parameters.Add(terminalDate);
            cmd.Parameters.Add(banknotes);
            cmd.Parameters.Add(values);

            cmd.ExecuteNonQuery();
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
        public void SaveTerminalStatus(int terminalId, int terminalStatus, int cashCodeStatus, int printerStatus, int cashcodeError, int cashcodeOutStatus, int cashcodeSuberror, int printerErrorState, int printerExtendedErrorState)
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
                                   "v_printer_extended_error_state => :v_printer_extended_error_state); " +
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
            currencies.Value = currencyList.ToArray();
            amounts.Value = amount.ToArray();

            currencies.Size = currencyList.Count;
            amounts.Size = amount.Count;

            cmd.Parameters.Add(terminalId);
            cmd.Parameters.Add(currencies);
            cmd.Parameters.Add(amounts);

            cmd.ExecuteNonQuery();
        }
    }
}
