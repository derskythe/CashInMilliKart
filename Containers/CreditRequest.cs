using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("StandardRequest")]
    [DataContract(Name = "StandardRequest", Namespace = "urn:CashIn")]
    public class CreditRequest : StandardRequest
    {
        private String _Phone;

        public CreditRequest(int terminalId) : base(terminalId)
        {
        }

        [XmlElement(ElementName = "Phone")]
        [DataMember(Name = "Phone")]
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public CreditRequest()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Phone: {1}", base.ToString(), _Phone);
        }
    }
}
