using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.CashCode;

namespace Containers
{
    [Serializable, XmlRoot("CashCodeDeviceStatus")]
    [DataContract(Name = "CashCodeDeviceStatus", Namespace = "urn:CashIn")]
    public class CashCodeDeviceStatus
    {
        private int _StateCode;
        private int _StateCodeOut;
        private int _ErrorCode;
        private int _SubErrorCode;
        private bool _BillEnable;
        private bool _FatalError;
        private string _DeviceStateDescription;
        private string _SubDeviceStateDescription;
        private bool _Init;

        [XmlElement(ElementName = "StateCode")]
        [DataMember(Name = "StateCode")]
        public int StateCode
        {
            get { return _StateCode; }
            set { _StateCode = value; }
        }

        [XmlElement(ElementName = "StateCodeOut")]
        [DataMember(Name = "StateCodeOut")]
        public int StateCodeOut
        {
            get { return _StateCodeOut; }
            set { _StateCodeOut = value; }
        }

        [XmlElement(ElementName = "ErrorCode")]
        [DataMember(Name = "ErrorCode")]
        public int ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        [XmlElement(ElementName = "SubErrorCode")]
        [DataMember(Name = "SubErrorCode")]
        public int SubErrorCode
        {
            get { return _SubErrorCode; }
            set { _SubErrorCode = value; }
        }

        [XmlElement(ElementName = "BillEnable")]
        [DataMember(Name = "BillEnable")]
        public bool BillEnable
        {
            get { return _BillEnable; }
            set { _BillEnable = value; }
        }

        [XmlElement(ElementName = "FatalError")]
        [DataMember(Name = "FatalError")]
        public bool FatalError
        {
            get { return _FatalError; }
            set { _FatalError = value; }
        }

        [XmlElement(ElementName = "DeviceStateDescription")]
        [DataMember(Name = "DeviceStateDescription")]
        public string DeviceStateDescription
        {
            get { return _DeviceStateDescription; }
            set { _DeviceStateDescription = value; }
        }

        [XmlElement(ElementName = "SubDeviceStateDescription")]
        [DataMember(Name = "SubDeviceStateDescription")]
        public string SubDeviceStateDescription
        {
            get { return _SubDeviceStateDescription; }
            set { _SubDeviceStateDescription = value; }
        }

        [XmlElement(ElementName = "Init")]
        [DataMember(Name = "Init")]
        public bool Init
        {
            get { return _Init; }
            set { _Init = value; }
        }

        public CashCodeDeviceStatus()
        {
        }

        public CashCodeDeviceStatus(CCNETResponseStatus stateCode, CCNETResponseStatus stateCodeOut, bool billEnable, bool fatalError, string deviceStateDescription, string subDeviceStateDescription, bool init)
        {
            _StateCode = (int)stateCode;
            _StateCodeOut = (int)stateCodeOut;
            _BillEnable = billEnable;
            _FatalError = fatalError;
            _DeviceStateDescription = deviceStateDescription;
            _SubDeviceStateDescription = subDeviceStateDescription;
            _Init = init;
        }

        public CashCodeDeviceStatus(int stateCode, int stateCodeOut, bool billEnable, bool fatalError, string deviceStateDescription, string subDeviceStateDescription, bool init)
        {
            _StateCode = stateCode;
            _StateCodeOut = stateCodeOut;
            _BillEnable = billEnable;
            _FatalError = fatalError;
            _DeviceStateDescription = deviceStateDescription;
            _SubDeviceStateDescription = subDeviceStateDescription;
            _Init = init;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "StateCode: {0}, StateCodeOut: {1}, ErrorCode: {2}, SubErrorCode: {3}, BillEnable: {4}, FatalError: {5}, DeviceStateDescription: {6}, SubDeviceStateDescription: {7}, Init: {8}",
                    _StateCode, _StateCodeOut, _ErrorCode, _SubErrorCode, _BillEnable, _FatalError,
                    _DeviceStateDescription, _SubDeviceStateDescription, _Init);
        }
    }
}
