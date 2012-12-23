using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("CommandResult")]
    [DataContract(Name = "CommandResult", Namespace = "urn:CashIn")]
    public class CommandResult : BaseMessage
    {
        private int _TerminalId;

        [XmlElement(ElementName = "TerminalId")]
        [DataMember(Name = "TerminalId")]
        public int TerminalId
        {
            get { return _TerminalId; }
            set { _TerminalId = value; }
        }

        public CommandResult()
        {
        }

        public CommandResult(int terminalId)
        {
            _TerminalId = terminalId;
        }

        public override string ToString()
        {
            return string.Format("{0}, TerminalId: {1}", base.ToString(), _TerminalId);
        }
    }
}
