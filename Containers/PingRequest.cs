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
        private int _PrinterStatus;
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

        [XmlElement(ElementName = "PrinterStatus")]
        [DataMember(Name = "PrinterStatus")]
        public int PrinterStatus
        {
            get { return _PrinterStatus; }
            set { _PrinterStatus = value; }
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

        public PingRequest(int terminalStatus, int cashCodeStatus, int printerStatus, string errorDescription)
        {
            _TerminalStatus = terminalStatus;
            _CashCodeStatus = cashCodeStatus;
            _PrinterStatus = printerStatus;
            _ErrorDescription = errorDescription;
        }

        public PingRequest(int terminalId, int terminalStatus, int cashCodeStatus, int printerStatus, string errorDescription)
            : base(terminalId)
        {
            _TerminalStatus = terminalStatus;
            _CashCodeStatus = cashCodeStatus;
            _PrinterStatus = printerStatus;
            _ErrorDescription = errorDescription;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, TerminalStatus: {1}, CashCodeStatus: {2}, PrinterStatus: {3}, ErrorDescription: {4}",
                    base.ToString(), _TerminalStatus, _CashCodeStatus, _PrinterStatus, _ErrorDescription);
        }
    }
}
