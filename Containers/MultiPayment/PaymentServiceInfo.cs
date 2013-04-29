using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServiceInfo")]
    [DataContract(Name = "PaymentServiceInfo", Namespace = "urn:CashIn")]
    public class PaymentServiceInfo
    {
        private int _ServiceId;
        private string _ServiceName;
        private List<PaymentServiceUserInputField> _Fields;

        [XmlElement(ElementName = "ServiceId")]
        [DataMember(Name = "ServiceId")]
        public int ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
        }

        [XmlElement(ElementName = "ServiceName")]
        [DataMember(Name = "ServiceName")]
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }
        }        

        [XmlArray("Fields")]
        [DataMember(Name = "Fields")]
        public List<PaymentServiceUserInputField> Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public PaymentServiceInfo()
        {
        }

        public PaymentServiceInfo(int serviceId, string serviceName, List<PaymentServiceUserInputField> fields)
        {
            _ServiceId = serviceId;
            _ServiceName = serviceName;
            _Fields = fields;
        }

        public override string ToString()
        {
            return string.Format("ServiceId: {0}, ServiceName: {1}, Fields: {2}", _ServiceId, _ServiceName,
                                 EnumEx.GetStringFromArray(_Fields));
        }
    }
}
