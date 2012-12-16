using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Db.dsTableAdapters
{
    public partial class V_LIST_TERMINAL_STATUSESTableAdapter
    {
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
                        result |= this.CommandCollection[i].BindByName;
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
                        this.CommandCollection[i].BindByName = value;
                    }
                }
            }
        }
    }
}
