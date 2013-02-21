using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("PrinterStatus")]
    [DataContract(Name = "PrinterStatus", Namespace = "urn:CashIn")]
    public class PrinterStatus
    {
        private int _Status;
        private int _ExtendedStatus;
        private int _ErrorState;
        private int _ExtendedErrorState;

        [XmlElement(ElementName = "Status")]
        [DataMember(Name = "Status")]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        [XmlElement(ElementName = "ExtendedStatus")]
        [DataMember(Name = "ExtendedStatus")]
        public int ExtendedStatus
        {
            get { return _ExtendedStatus; }
            set { _ExtendedStatus = value; }
        }

        [XmlElement(ElementName = "ErrorState")]
        [DataMember(Name = "ErrorState")]
        public int ErrorState
        {
            get { return _ErrorState; }
            set { _ErrorState = value; }
        }

        [XmlElement(ElementName = "ExtendedErrorState")]
        [DataMember(Name = "ExtendedErrorState")]
        public int ExtendedErrorState
        {
            get { return _ExtendedErrorState; }
            set { _ExtendedErrorState = value; }
        }

        public PrinterStatus()
        {
        }

        public PrinterStatus(int status, int extendedStatus, int errorState, int extendedErrorState)
        {
            _Status = status;
            _ExtendedStatus = extendedStatus;
            _ErrorState = errorState;
            _ExtendedErrorState = extendedErrorState;
        }

        public override string ToString()
        {
            return string.Format("Status: {0}, ExtendedStatus: {1}, ErrorState: {2}, ExtendedErrorState: {3}", _Status,
                                 _ExtendedStatus, _ErrorState, _ExtendedErrorState);
        }
    }
}
