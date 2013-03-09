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
        private int _CommandResult;

        [XmlElement(ElementName = "TerminalId")]
        [DataMember(Name = "TerminalId")]
        public int TerminalId
        {
            get { return _TerminalId; }
            set { _TerminalId = value; }
        }

        [XmlElement(ElementName = "CommandResult")]
        [DataMember(Name = "CommandResult")]
        public int CommandResult
        {
            get { return _CommandResult; }
            set { _CommandResult = value; }
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
            return string.Format("{0}, TerminalId: {1}, CommandResult: {2}", base.ToString(), _TerminalId,
                                 _CommandResult);
        }
    }
}
