using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("TerminalVersionRequest")]
    [DataContract(Name = "TerminalVersionRequest", Namespace = "urn:CashIn")]
    public class TerminalVersionRequest : StandardRequest
    {
        private String _Version;

        public TerminalVersionRequest(int terminalId) : base(terminalId)
        {
        }

        [XmlElement(ElementName = "Version")]
        [DataMember(Name = "Version")]
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        public TerminalVersionRequest()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Version: {1}", base.ToString(), _Version);
        }
    }
}
