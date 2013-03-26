using System;
using System.Collections.Generic;
using System.Data.SQLite;
using CashInTerminal.Properties;
using CashInTerminal.dsTableAdapters;

namespace CashInTerminal
{
    public class LocalDb
    {
        private readonly SQLiteConnection _Connection;

        public LocalDb()
        {
            _Connection = new SQLiteConnection("Data Source=" + Settings.Default.DbPath);
        }

        public List<ds.PaymentsRow> GetTransactions()
        {
            var adapter = new PaymentsTableAdapter { Connection = _Connection };
            var table = new ds.PaymentsDataTable();

            adapter.FillByConfirmed(table);

            var rowList = new List<ds.PaymentsRow>();

            foreach (ds.PaymentsRow item in table.Rows)
            {
                rowList.Add(item);
            }

            return rowList;
        }

        public int GetCasseteTotal(string currency)
        {
            var adapter = new CasseteBanknotesTableAdapter {Connection = _Connection};

            var raw = adapter.Total(currency);

            if (raw != null && raw != DBNull.Value)
            {
                return Convert.ToInt32(raw);
            }

            return 0;
        }

        public List<ds.CasseteBanknotesRow> ListCasseteBanknotes()
        {
            var adapter = new CasseteBanknotesTableAdapter {Connection = _Connection};
            var table = new ds.CasseteBanknotesDataTable();

            adapter.Fill(table);

            var rowList = new List<ds.CasseteBanknotesRow>();

            foreach (ds.CasseteBanknotesRow row in table.Rows)
            {
                rowList.Add(row);
            }

            return rowList;
        } 

        public List<ds.PaymentBanknotesRow> GetPaymentBanknotes(long id)
        {
            var adapter = new PaymentBanknotesTableAdapter {Connection = _Connection};
            var table = new ds.PaymentBanknotesDataTable();

            adapter.FillByParentId(table, id);

            var rowList = new List<ds.PaymentBanknotesRow>();

            foreach (ds.PaymentBanknotesRow row in table.Rows)
            {
                rowList.Add(row);
            }

            return rowList;
        }

        public List<ds.PaymentValuesRow> GetPaymentValues(long id)
        {
            var adapter = new PaymentValuesTableAdapter {Connection = _Connection};
            var table = new ds.PaymentValuesDataTable();

            adapter.FillByParentId(table, id);

            var rowList = new List<ds.PaymentValuesRow>();

            foreach (ds.PaymentValuesRow row in table.Rows)
            {
                rowList.Add(row);
            }

            return rowList;
        }

        public List<ds.CheckTemplateRow> GetCheckTemplateByType(long type, String language)
        {
            var adapter = new CheckTemplateTableAdapter { Connection = _Connection };
            var table = new ds.CheckTemplateDataTable();

            adapter.FillByType(table, type, language);

            var rowList = new List<ds.CheckTemplateRow>();
            foreach (ds.CheckTemplateRow row in table.Rows)
            {
                rowList.Add(row);
            }

            return rowList;
        }

        public List<ds.TemplateFieldRow> ListTemplateFields(long checkTemplateId)
        {
            var adapter = new TemplateFieldTableAdapter { Connection = _Connection };
            var table = new ds.TemplateFieldDataTable();

            adapter.FillByCheckTemplateId(table, checkTemplateId);
            var rowList = new List<ds.TemplateFieldRow>();

            foreach (ds.TemplateFieldRow row in table.Rows)
            {
                rowList.Add(row);
            }

            return rowList;
        }

        public void DeleteTemplateByType(long type, String language)
        {
            var templates = GetCheckTemplateByType(type, language);

            foreach (ds.CheckTemplateRow row in templates)
            {
                DeleteByCheckTemplateId(row.Id);
            }

            var adapter = new CheckTemplateTableAdapter { Connection = _Connection };
            adapter.DeleteTemplateByType(type, language);
        }

        public void DeleteByCheckTemplateId(long id)
        {
            var adapter = new TemplateFieldTableAdapter { Connection = _Connection };
            adapter.DeleteByCheckTemplateId(id);
        }

        public void UpdateTemplate(long id, DateTime updateDate)
        {
            var adapter = new CheckTemplateTableAdapter { Connection = _Connection };
            adapter.UpdateTemplate(updateDate, id);
        }

        public void InsertTemplate(long id, long type, String language, DateTime updateDate)
        {
            var adapter = new CheckTemplateTableAdapter { Connection = _Connection };
            adapter.InsertTemplate(id, type, language, updateDate);
        }

        public ds.CheckTemplateRow GetCheckTemplate(long id)
        {
            var adapter = new CheckTemplateTableAdapter { Connection = _Connection };
            var table = new ds.CheckTemplateDataTable();

            adapter.FillById(table, id);
            foreach (ds.CheckTemplateRow row in table.Rows)
            {
                return row;
            }

            return null;
        }

        public void InsertCheckTemplateField(long id, long parentId, long type, String value, byte[] image, long orderNumber)
        {
            var adapter = new TemplateFieldTableAdapter { Connection = _Connection };
            adapter.InsertCheckTemplateField(id, parentId, type, value, image, orderNumber);
        }

        public void DeleteTransaction(long id)
        {
            var adapter = new PaymentsTableAdapter { Connection = _Connection };
            adapter.DeleteTransaction(id);

            var banknoteAdapter = new PaymentBanknotesTableAdapter { Connection = _Connection };
            banknoteAdapter.DeleteBanknotes(id);

            var paymentValuesAdapter = new PaymentValuesTableAdapter { Connection = _Connection };
            paymentValuesAdapter.DeleteValuesBytTransaction(id);
        }

        public long InsertTransaction(long productId, string currency, decimal currencyRate, int amount, int terminalId,
                                      bool confirmed)
        {
            var adapter = new PaymentsTableAdapter { Connection = _Connection };
            adapter.InsertTransaction(productId, currency, currencyRate, amount, null, confirmed ? 1 : 0);

            var insertId = adapter.GetInsertId();

            if (insertId != null)
            {
                return Convert.ToInt64(insertId);
            }

            return 0;
        }

        public void UpdateAmount(long id, string currency, decimal currencyRate, int amount)
        {
            var adapter = new PaymentsTableAdapter { Connection = _Connection };
            adapter.UpdateAmount(currency, currencyRate, amount, id);
        }

        public void UpdateTransactionId(long id, string transId)
        {
            var adapter = new PaymentsTableAdapter { Connection = _Connection };
            adapter.UpdateTransactionId(transId, id);
        }

        public void ConfirmTransaction(long id)
        {
            var adapter = new PaymentsTableAdapter { Connection = _Connection };
            adapter.UpdateConfirmed(1, id);
        }

        public void InsertBanknote(long parentId, int amount, string currency, int orderNumber)
        {
            var adapter = new PaymentBanknotesTableAdapter { Connection = _Connection };
            adapter.InsertBanknote(parentId, amount, currency, orderNumber);
        }

        public void InsertPaymentValue(long parentId, string value, int orderNumber)
        {
            var adapter = new PaymentValuesTableAdapter {Connection = _Connection};
            adapter.InsertPaymentValue(parentId, value, orderNumber);
        }

        public void DeleteCasseteBanknotes()
        {
            var adapter = new CasseteBanknotesTableAdapter {Connection = _Connection};
            adapter.DeleteAll();
        }

        public void InsertTransactionBanknotes(int amount, String currency, String transId)
        {
            var adapter = new CasseteBanknotesTableAdapter { Connection = _Connection };
            adapter.Insert(DateTime.Now, currency, transId, amount);
        }

        public int CountCasseteBanknotes()
        {
            var adapter = new CasseteBanknotesTableAdapter { Connection = _Connection };
            var result = adapter.Count();

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
    }
}
