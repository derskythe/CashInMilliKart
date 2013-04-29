using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentServiceInfoRequest")]
    [DataContract(Name = "PaymentServiceInfoRequest", Namespace = "urn:CashIn")]
    public class PaymentServiceInfoRequest : StandardRequest
    {
        private PaymentServiceInfo _Info;

        public PaymentServiceInfoRequest(int terminalId)
            : base(terminalId)
        {
        }

        [XmlElement(ElementName = "Info")]
        [DataMember(Name = "Info")]
        public PaymentServiceInfo Info
        {
            get { return _Info; }
            set { _Info = value; }
        }

        public PaymentServiceInfoRequest()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Info: {1}", base.ToString(), _Info);
        }
    }
}
