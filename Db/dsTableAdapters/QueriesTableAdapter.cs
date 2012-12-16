using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Db.dsTableAdapters
{
    public partial class QueriesTableAdapter : global::System.ComponentModel.Component
    {
        private global::Oracle.DataAccess.Client.OracleConnection _connection;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal global::Oracle.DataAccess.Client.OracleConnection Connection
        {
            get
            {
                return this._connection;
            }
            set
            {
                this._connection = value;
                for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1))
                {
                    if ((this.CommandCollection[i] != null))
                    {
                        ((global::Oracle.DataAccess.Client.OracleCommand)(this.CommandCollection[i])).Connection = value;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        public bool BindByName
        {
            get
            {
                bool result = false;
                for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1))
                {
                    if ((this.CommandCollection[i] != null))
                    {
                        result |= ((global::Oracle.DataAccess.Client.OracleCommand)(this.CommandCollection[i])).BindByName;
                    }
                }

                return result;
            }
            set
            {
                for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1))
                {
                    if ((this.CommandCollection[i] != null))
                    {
                        ((global::Oracle.DataAccess.Client.OracleCommand)(this.CommandCollection[i])).BindByName = value;
                    }
                }
            }
        }
    }
}
