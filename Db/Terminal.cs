using System.Collections.Generic;
using Containers;
using Db.dsTableAdapters;

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

        public Terminal GetTerminal(int id)
        {
            CheckConnection();

            var adapter = new V_LIST_TERMINALSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_LIST_TERMINALSDataTable();
            adapter.Fill(table);

            foreach (ds.V_LIST_TERMINALSRow item in table.Rows)
            {
                return Convertor.ToTerminal(item);
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

        public void SavePayment()
        {

        }

        public void UpdateTerminalStatus(int terminalStatus, int cashCodeStatus)
        {

        }
    }
}
