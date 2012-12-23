using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("StandardRequest")]
    [DataContract(Name = "StandardRequest", Namespace = "urn:CashIn")]
    public class StandardRequest : BaseMessage
    {
        protected int _TerminalId;

        [XmlElement(ElementName = "TerminalId")]
        [DataMember(Name = "TerminalId")]
        public int TerminalId
        {
            get { return _TerminalId; }
            set { _TerminalId = value; }
        }

        public StandardRequest()
        {
        }

        public StandardRequest(int terminalId)
        {
            _TerminalId = terminalId;
        }

        public override string ToString()
        {
            return string.Format("{0}, TerminalId: {1}", base.ToString(), _TerminalId);
        }
    }
}
