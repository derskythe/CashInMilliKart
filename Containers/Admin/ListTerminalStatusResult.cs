using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListTerminalStatusResult")]
    [DataContract(Name = "ListTerminalStatusResult", Namespace = "urn:CashIn")]
    public class ListTerminalStatusResult : StandardResult
    {
        private List<TerminalStatusCode> _Statuses;

        [XmlArray("Statuses")]
        [DataMember(Name = "Statuses")]
        public List<TerminalStatusCode> Statuses
        {
            get { return _Statuses; }
            set { _Statuses = value; }
        }

        public ListTerminalStatusResult()
        {
        }

        public ListTerminalStatusResult(List<TerminalStatusCode> statuses)
        {
            _Statuses = statuses;
        }

        public override string ToString()
        {
            return string.Format("{0}, Statuses: {1}", base.ToString(), _Statuses);
        }
    }
}
