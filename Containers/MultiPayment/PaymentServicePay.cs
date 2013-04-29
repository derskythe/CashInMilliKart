using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServicePay")]
    [DataContract(Name = "PaymentServicePay", Namespace = "urn:CashIn")]
    public class PaymentServicePay : PaymentServiceInfo
    {
        private string _PaymentId;
        private int _Amount;
        private string _Currency;

        public PaymentServicePay(int serviceId, string serviceName, List<PaymentServiceUserInputField> fields)
            : base(serviceId, serviceName, fields)
        {
        }

        [XmlElement(ElementName = "PaymentId")]
        [DataMember(Name = "PaymentId")]
        public string PaymentId
        {
            get { return _PaymentId; }
            set { _PaymentId = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        public PaymentServicePay()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, PaymentId: {1}, Amount: {2}, Currency: {3}", base.ToString(), _PaymentId, _Amount,
                                 _Currency);
        }
    }
}
