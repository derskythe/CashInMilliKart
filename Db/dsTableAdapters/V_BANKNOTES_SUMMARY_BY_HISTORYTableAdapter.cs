using System.ComponentModel;

namespace Db.dsTableAdapters
{
    public partial class V_BANKNOTES_SUMMARY_BY_HISTORYTableAdapter
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        public bool BindByName
        {
            get
            {
                bool result = false;
                for (int i = 0; (i < CommandCollection.Length); i = (i + 1))
                {
                    if ((CommandCollection[i] != null))
                    {
                        result |= CommandCollection[i].BindByName;
                    }
                }

                return result;
            }
            set
            {
                for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1))
                {
                    if ((CommandCollection[i] != null))
                    {
                        CommandCollection[i].BindByName = value;
                    }
                }
            }
        }
    }
}
