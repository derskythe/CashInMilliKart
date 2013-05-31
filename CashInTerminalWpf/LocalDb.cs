using System;
using System.Collections.Generic;
using System.Data.SQLite;
using CashInTerminalWpf.Properties;
using CashInTerminalWpf.dsTableAdapters;

namespace CashInTerminalWpf
{
    public class LocalDb
    {
        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=" + Settings.Default.DbPath);
        }

        public List<ds.PaymentsRow> GetTransactions()
        {
            var rowList = new List<ds.PaymentsRow>();
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    using (var table = new ds.PaymentsDataTable())
                    {
                        adapter.FillByConfirmed(table);

                        foreach (ds.PaymentsRow item in table.Rows)
                        {
                            rowList.Add(item);
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

            return rowList;
        }

        public void UpdateOrphanedTransactions()
        {
            var connection = GetConnection();
            try
            {                
                using (var adapter = new PaymentsTableAdapter {Connection = connection})
                {
                    using (var table = new ds.PaymentsDataTable())
                    {
                        adapter.FillByNonConfirmed(table);

                        foreach (ds.PaymentsRow item in table.Rows)
                        {
                            var amount = CountTransactionAmount(item.Id);

                            if (amount > 0)
                            {
                                UpdateAmount(item.Id, item.Currency, 1, amount);
                                ConfirmTransaction(item.Id);
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
        }

        public int CountTransactionAmount(long parentId)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentBanknotesTableAdapter { Connection = connection })
                {
                    var result = adapter.CountAmount(parentId);

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
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

        public int GetCasseteTotal(string currency)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CasseteBanknotesTableAdapter { Connection = connection })
                {
                    var raw = adapter.Total(currency);

                    if (raw != null && raw != DBNull.Value)
                    {
                        return Convert.ToInt32(raw);
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

        public List<ds.CasseteBanknotesRow> ListCasseteBanknotes()
        {
            var rowList = new List<ds.CasseteBanknotesRow>();

            var connection = GetConnection();
            try
            {
                using (var adapter = new CasseteBanknotesTableAdapter { Connection = connection })
                {
                    using (var table = new ds.CasseteBanknotesDataTable())
                    {

                        adapter.Fill(table);

                        foreach (ds.CasseteBanknotesRow row in table.Rows)
                        {
                            rowList.Add(row);
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

            return rowList;
        }

        public List<ds.PaymentBanknotesRow> GetPaymentBanknotes(long id)
        {
            var rowList = new List<ds.PaymentBanknotesRow>();

            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentBanknotesTableAdapter { Connection = connection })
                {
                    using (var table = new ds.PaymentBanknotesDataTable())
                    {
                        adapter.FillByParentId(table, id);

                        foreach (ds.PaymentBanknotesRow row in table.Rows)
                        {
                            rowList.Add(row);
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

            return rowList;
        }

        public List<ds.PaymentValuesRow> GetPaymentValues(long id)
        {
            var rowList = new List<ds.PaymentValuesRow>();

            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentValuesTableAdapter { Connection = connection })
                {
                    using (var table = new ds.PaymentValuesDataTable())
                    {
                        adapter.FillByParentId(table, id);

                        foreach (ds.PaymentValuesRow row in table.Rows)
                        {
                            rowList.Add(row);
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

            return rowList;
        }

        public List<ds.CheckTemplateRow> GetCheckTemplateByType(long type, String language)
        {
            var rowList = new List<ds.CheckTemplateRow>();

            var connection = GetConnection();
            try
            {
                using (var adapter = new CheckTemplateTableAdapter { Connection = connection })
                {
                    using (var table = new ds.CheckTemplateDataTable())
                    {
                        adapter.FillByType(table, type, language);

                        foreach (ds.CheckTemplateRow row in table.Rows)
                        {
                            rowList.Add(row);
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

            return rowList;
        }

        public List<ds.TemplateFieldRow> ListTemplateFields(long checkTemplateId)
        {
            var rowList = new List<ds.TemplateFieldRow>();
            var connection = GetConnection();
            try
            {
                using (var adapter = new TemplateFieldTableAdapter { Connection = connection })
                {
                    using (var table = new ds.TemplateFieldDataTable())
                    {

                        adapter.FillByCheckTemplateId(table, checkTemplateId);
                        foreach (ds.TemplateFieldRow row in table.Rows)
                        {
                            rowList.Add(row);
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

            return rowList;
        }

        public void DeleteTemplateByType(long type, String language)
        {
            var templates = GetCheckTemplateByType(type, language);

            foreach (ds.CheckTemplateRow row in templates)
            {
                DeleteByCheckTemplateId(row.Id);
            }

            var connection = GetConnection();
            try
            {
                using (var adapter = new CheckTemplateTableAdapter { Connection = connection })
                {
                    adapter.DeleteTemplateByType(type, language);
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

        public void DeleteByCheckTemplateId(long id)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new TemplateFieldTableAdapter { Connection = connection })
                {
                    adapter.DeleteByCheckTemplateId(id);
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

        public void UpdateNonConfirmed(DateTime beforeDate)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    adapter.UpdateNonConfirmed(beforeDate);
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

        public void UpdateTemplate(long id, DateTime updateDate)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CheckTemplateTableAdapter { Connection = connection })
                {
                    adapter.UpdateTemplate(updateDate, id);
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

        public void InsertTemplate(long id, long type, String language, DateTime updateDate)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CheckTemplateTableAdapter { Connection = connection })
                {
                    adapter.InsertTemplate(id, type, language, updateDate);
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

        public ds.CheckTemplateRow GetCheckTemplate(long id)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CheckTemplateTableAdapter { Connection = connection })
                {
                    using (var table = new ds.CheckTemplateDataTable())
                    {
                        adapter.FillById(table, id);
                        foreach (ds.CheckTemplateRow row in table.Rows)
                        {
                            return row;
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

        public List<ds.CountBanknotesRow> CountAllBanknotes()
        {
            var result = new List<ds.CountBanknotesRow>();

            var connection = GetConnection();
            try
            {
                using (var adapter = new CountBanknotesTableAdapter { Connection = connection })
                {
                    using (var table = new ds.CountBanknotesDataTable())
                    {
                        adapter.Fill(table);
                        foreach (ds.CountBanknotesRow row in table.Rows)
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

            return result;
        }

        public void InsertCheckTemplateField(long id, long parentId, long type, String value, byte[] image, long orderNumber)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new TemplateFieldTableAdapter { Connection = connection })
                {
                    adapter.InsertCheckTemplateField(id, parentId, type, value, image, orderNumber);
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

        public void DeleteTransaction(long id)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    adapter.DeleteTransaction(id);
                }

                using (var banknoteAdapter = new PaymentBanknotesTableAdapter { Connection = connection })
                {
                    banknoteAdapter.DeleteBanknotes(id);
                }

                using (var paymentValuesAdapter = new PaymentValuesTableAdapter { Connection = connection })
                {
                    paymentValuesAdapter.DeleteValuesBytTransaction(id);
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

        public long InsertTransaction(long productId, long serviceId, string currency, decimal currencyRate, int amount, int terminalId,
            string creditNumber, int operationType, bool confirmed)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    adapter.InsertTransaction(productId, currency, currencyRate, amount, null, confirmed ? 1 : 0,
                                              operationType, creditNumber, serviceId);

                    var insertId = adapter.GetInsertId();

                    if (insertId != null)
                    {
                        return Convert.ToInt64(insertId);
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

        public void UpdateAmount(long id, string currency, decimal currencyRate, int amount)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    adapter.UpdateAmount(currency, currencyRate, amount, id);
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

        public void UpdateTransactionId(long id, string transId)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    adapter.UpdateTransactionId(transId, id);
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

        public void ConfirmTransaction(long id)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentsTableAdapter { Connection = connection })
                {
                    adapter.UpdateConfirmed(1, id);
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

        public void InsertBanknote(long parentId, int amount, string currency, int orderNumber)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentBanknotesTableAdapter { Connection = connection })
                {
                    adapter.InsertBanknote(parentId, amount, currency, orderNumber);
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

        public void InsertPaymentValue(long parentId, string value, int orderNumber)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new PaymentValuesTableAdapter { Connection = connection })
                {
                    adapter.InsertPaymentValue(parentId, value, orderNumber);
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

        public void DeleteCasseteBanknotes()
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CasseteBanknotesTableAdapter { Connection = connection })
                {
                    adapter.DeleteAll();
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

        public void InsertTransactionBanknotes(int amount, String currency, String transId)
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CasseteBanknotesTableAdapter { Connection = connection })
                {
                    adapter.Insert(DateTime.Now, currency, transId, amount);
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

        public int CountCasseteBanknotes()
        {
            var connection = GetConnection();
            try
            {
                using (var adapter = new CasseteBanknotesTableAdapter { Connection = connection })
                {
                    var result = adapter.Count();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
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
    }
}
