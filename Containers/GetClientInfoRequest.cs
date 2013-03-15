using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers
{
    [Serializable, XmlRoot("GetClientInfoRequest")]
    [DataContract(Name = "GetClientInfoRequest", Namespace = "urn:CashIn")]
    public class GetClientInfoRequest : StandardRequest
    {
        private String _CreditAccount; 
        private String _PasportNumber;
        private String _ClientCode;
        private String _Bolcard8Digits;
        private GetClientInfoType _ClientInfoType;

        public GetClientInfoRequest(int terminalId) : base(terminalId)
        {
        }

        [XmlElement(ElementName = "CreditAccount")]
        [DataMember(Name = "CreditAccount")]
        public string CreditAccount
        {
            get { return _CreditAccount; }
            set { _CreditAccount = value; }
        }

        [XmlElement(ElementName = "PasportNumber")]
        [DataMember(Name = "PasportNumber")]
        public string PasportNumber
        {
            get { return _PasportNumber; }
            set { _PasportNumber = value; }
        }

        [XmlElement(ElementName = "ClientCode")]
        [DataMember(Name = "ClientCode")]
        public string ClientCode
        {
            get { return _ClientCode; }
            set { _ClientCode = value; }
        }

        [XmlElement(ElementName = "ClientInfoType")]
        [DataMember(Name = "ClientInfoType")]
        public GetClientInfoType ClientInfoType
        {
            get { return _ClientInfoType; }
            set { _ClientInfoType = value; }
        }

        [XmlElement(ElementName = "Bolcard8Digits")]
        [DataMember(Name = "Bolcard8Digits")]
        public string Bolcard8Digits
        {
            get { return _Bolcard8Digits; }
            set { _Bolcard8Digits = value; }
        }

        public GetClientInfoRequest()
        {
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}, CreditAccount: {1}, PasportNumber: {2}, ClientCode: {3}, Bolcard8Digits: {4}, ClientInfoType: {5}",
                    base.ToString(), _CreditAccount, _PasportNumber, _ClientCode, _Bolcard8Digits, _ClientInfoType);
        }
    }
}
