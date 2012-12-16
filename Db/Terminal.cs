using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db.dsTableAdapters;

namespace Db
{
    public partial class Db
    {
        public List<ds.V_PRODUCTSRow> ListProducts()
        {
            CheckConnection();

            var adapter = new V_PRODUCTSTableAdapter { Connection = _OraCon, BindByName = true };

            var table = new ds.V_PRODUCTSDataTable();
            adapter.Fill(table);

            var result = new List<ds.V_PRODUCTSRow>();

            foreach (ds.V_PRODUCTSRow item in table.Rows)
            {
                result.Add(item);
            }

            return result;
        }
    }
}
