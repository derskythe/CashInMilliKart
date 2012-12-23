using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("PingRequest")]
    [DataContract(Name = "PingRequest", Namespace = "urn:CashIn")]
    public class PingRequest : StandardRequest
    {
        private int _TerminalStatus;
        private int _CashCodeStatus;
        private String _ErrorDescription;

        [XmlElement(ElementName = "TerminalStatus")]
        [DataMember(Name = "TerminalStatus")]
        public int TerminalStatus
        {
            get { return _TerminalStatus; }
            set { _TerminalStatus = value; }
        }

        [XmlElement(ElementName = "CashCodeStatus")]
        [DataMember(Name = "CashCodeStatus")]
        public int CashCodeStatus
        {
            get { return _CashCodeStatus; }
            set { _CashCodeStatus = value; }
        }

        [XmlElement(ElementName = "ErrorDescription")]
        [DataMember(Name = "ErrorDescription")]
        public string ErrorDescription
        {
            get { return _ErrorDescription; }
            set { _ErrorDescription = value; }
        }

        public PingRequest()
        {
        }

        public PingRequest(int terminalId, int terminalStatus, int cashCodeStatus, string errorDescription, string sign)
        {
            _TerminalId = terminalId;
            _TerminalStatus = terminalStatus;
            _CashCodeStatus = cashCodeStatus;
            _ErrorDescription = errorDescription;
            _Sign = sign;
        }

        public override string ToString()
        {
            return string.Format(
                "{0}, TerminalId: {1}, TerminalStatus: {2}, CashCodeStatus: {3}, ErrorDescription: {4}", base.ToString(),
                _TerminalId, _TerminalStatus, _CashCodeStatus, _ErrorDescription);
        }
    }
}
