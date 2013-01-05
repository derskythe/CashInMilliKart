using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("TerminalInfoResult")]
    [DataContract(Name = "TerminalInfoResult", Namespace = "urn:CashIn")]
    public class TerminalInfoResult : StandardResult
    {
        private Terminal _Terminal;

        [XmlElement(ElementName = "Terminal")]
        [DataMember(Name = "Terminal")]
        public Terminal Terminal
        {
            get { return _Terminal; }
            set { _Terminal = value; }
        }

        public TerminalInfoResult()
        {
        }

        public TerminalInfoResult(Terminal terminal)
        {
            _Terminal = terminal;
        }

        public override string ToString()
        {
            return string.Format("{0}, Terminal: {1}", base.ToString(), _Terminal);
        }
    }
}
