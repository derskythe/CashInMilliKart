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
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                var table = new ds.PaymentsDataTable();

                adapter.FillByConfirmed(table);

                foreach (ds.PaymentsRow item in table.Rows)
                {
                    rowList.Add(item);
                }

                connection.Close();
            }

            return rowList;
        }

        public int GetCasseteTotal(string currency)
        {
            using (var connection = GetConnection())
            {
                var adapter = new CasseteBanknotesTableAdapter { Connection = connection };

                var raw = adapter.Total(currency);

                if (raw != null && raw != DBNull.Value)
                {
                    connection.Close();
                    return Convert.ToInt32(raw);
                }
            }
            return 0;
        }

        public List<ds.CasseteBanknotesRow> ListCasseteBanknotes()
        {
            var rowList = new List<ds.CasseteBanknotesRow>();

            using (var connection = GetConnection())
            {
                var adapter = new CasseteBanknotesTableAdapter { Connection = connection };
                var table = new ds.CasseteBanknotesDataTable();

                adapter.Fill(table);

                foreach (ds.CasseteBanknotesRow row in table.Rows)
                {
                    rowList.Add(row);
                }

                connection.Close();
            }

            return rowList;
        }

        public List<ds.PaymentBanknotesRow> GetPaymentBanknotes(long id)
        {
            var rowList = new List<ds.PaymentBanknotesRow>();

            using (var connection = GetConnection())
            {
                var adapter = new PaymentBanknotesTableAdapter { Connection = connection };
                var table = new ds.PaymentBanknotesDataTable();

                adapter.FillByParentId(table, id);

                foreach (ds.PaymentBanknotesRow row in table.Rows)
                {
                    rowList.Add(row);
                }
                connection.Close();
            }

            return rowList;
        }

        public List<ds.PaymentValuesRow> GetPaymentValues(long id)
        {
            var rowList = new List<ds.PaymentValuesRow>();

            using (var connection = GetConnection())
            {
                var adapter = new PaymentValuesTableAdapter { Connection = connection };
                var table = new ds.PaymentValuesDataTable();

                adapter.FillByParentId(table, id);

                foreach (ds.PaymentValuesRow row in table.Rows)
                {
                    rowList.Add(row);
                }

                connection.Close();
            }

            return rowList;
        }

        public List<ds.CheckTemplateRow> GetCheckTemplateByType(long type, String language)
        {
            var rowList = new List<ds.CheckTemplateRow>();

            using (var connection = GetConnection())
            {
                var adapter = new CheckTemplateTableAdapter { Connection = connection };
                var table = new ds.CheckTemplateDataTable();

                adapter.FillByType(table, type, language);

                foreach (ds.CheckTemplateRow row in table.Rows)
                {
                    rowList.Add(row);
                }

                connection.Close();
            }

            return rowList;
        }

        public List<ds.TemplateFieldRow> ListTemplateFields(long checkTemplateId)
        {
            var rowList = new List<ds.TemplateFieldRow>();
            using (var connection = GetConnection())
            {
                var adapter = new TemplateFieldTableAdapter { Connection = connection };
                var table = new ds.TemplateFieldDataTable();

                adapter.FillByCheckTemplateId(table, checkTemplateId);


                foreach (ds.TemplateFieldRow row in table.Rows)
                {
                    rowList.Add(row);
                }

                connection.Close();
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

            using (var connection = GetConnection())
            {
                var adapter = new CheckTemplateTableAdapter { Connection = connection };
                adapter.DeleteTemplateByType(type, language);
                connection.Close();
            }
        }

        public void DeleteByCheckTemplateId(long id)
        {
            using (var connection = GetConnection())
            {
                var adapter = new TemplateFieldTableAdapter { Connection = connection };
                adapter.DeleteByCheckTemplateId(id);
                connection.Close();
            }
        }

        public void UpdateNonConfirmed(DateTime beforeDate)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                adapter.UpdateNonConfirmed(beforeDate);
                connection.Close();
            }            
        }

        public void UpdateTemplate(long id, DateTime updateDate)
        {
            using (var connection = GetConnection())
            {
                var adapter = new CheckTemplateTableAdapter { Connection = connection };
                adapter.UpdateTemplate(updateDate, id);
                connection.Close();
            }
        }

        public void InsertTemplate(long id, long type, String language, DateTime updateDate)
        {
            using (var connection = GetConnection())
            {
                var adapter = new CheckTemplateTableAdapter { Connection = connection };
                adapter.InsertTemplate(id, type, language, updateDate);
                connection.Close();
            }
        }

        public ds.CheckTemplateRow GetCheckTemplate(long id)
        {
            using (var connection = GetConnection())
            {
                var adapter = new CheckTemplateTableAdapter { Connection = connection };
                var table = new ds.CheckTemplateDataTable();

                adapter.FillById(table, id);
                foreach (ds.CheckTemplateRow row in table.Rows)
                {
                    connection.Close();
                    return row;
                }
            }

            return null;
        }

        public List<ds.CountBanknotesRow> CountAllBanknotes()
        {
            var result = new List<ds.CountBanknotesRow>();

            using (var connection = GetConnection())
            {
                var adapter = new CountBanknotesTableAdapter { Connection = connection };
                var table = new ds.CountBanknotesDataTable();

                adapter.Fill(table);
                foreach (ds.CountBanknotesRow row in table.Rows)
                {                    
                    result.Add(row);
                }

                connection.Close();
            }

            return result;
        }

        public void InsertCheckTemplateField(long id, long parentId, long type, String value, byte[] image, long orderNumber)
        {
            using (var connection = GetConnection())
            {
                var adapter = new TemplateFieldTableAdapter { Connection = connection };
                adapter.InsertCheckTemplateField(id, parentId, type, value, image, orderNumber);
                connection.Close();
            }
        }

        public void DeleteTransaction(long id)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                adapter.DeleteTransaction(id);

                var banknoteAdapter = new PaymentBanknotesTableAdapter { Connection = connection };
                banknoteAdapter.DeleteBanknotes(id);

                var paymentValuesAdapter = new PaymentValuesTableAdapter { Connection = connection };
                paymentValuesAdapter.DeleteValuesBytTransaction(id);

                connection.Close();
            }
        }

        public long InsertTransaction(long productId, string currency, decimal currencyRate, int amount, int terminalId,
            string creditNumber, int operationType, bool confirmed)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                adapter.InsertTransaction(productId, currency, currencyRate, amount, null, confirmed ? 1 : 0,
                                          operationType, creditNumber);

                var insertId = adapter.GetInsertId();

                if (insertId != null)
                {
                    connection.Close();
                    return Convert.ToInt64(insertId);
                }
            }

            return 0;
        }

        public void UpdateAmount(long id, string currency, decimal currencyRate, int amount)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                adapter.UpdateAmount(currency, currencyRate, amount, id);
                connection.Close();
            }
        }

        public void UpdateTransactionId(long id, string transId)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                adapter.UpdateTransactionId(transId, id);
                connection.Close();
            }
        }

        public void ConfirmTransaction(long id)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentsTableAdapter { Connection = connection };
                adapter.UpdateConfirmed(1, id);
                connection.Close();
            }
        }

        public void InsertBanknote(long parentId, int amount, string currency, int orderNumber)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentBanknotesTableAdapter { Connection = connection };
                adapter.InsertBanknote(parentId, amount, currency, orderNumber);
                connection.Close();
            }
        }

        public void InsertPaymentValue(long parentId, string value, int orderNumber)
        {
            using (var connection = GetConnection())
            {
                var adapter = new PaymentValuesTableAdapter { Connection = connection };
                adapter.InsertPaymentValue(parentId, value, orderNumber);
                connection.Close();
            }
        }

        public void DeleteCasseteBanknotes()
        {
            using (var connection = GetConnection())
            {
                var adapter = new CasseteBanknotesTableAdapter { Connection = connection };
                adapter.DeleteAll();
                connection.Close();
            }
        }

        public void InsertTransactionBanknotes(int amount, String currency, String transId)
        {
            using (var connection = GetConnection())
            {
                var adapter = new CasseteBanknotesTableAdapter { Connection = connection };
                adapter.Insert(DateTime.Now, currency, transId, amount);
                connection.Close();
            }
        }

        public int CountCasseteBanknotes()
        {
            using (var connection = GetConnection())
            {
                var adapter = new CasseteBanknotesTableAdapter { Connection = connection };
                var result = adapter.Count();

                if (result != null)
                {
                    connection.Close();
                    return Convert.ToInt32(result);
                }
            }

            return 0;
        }
    }
}
