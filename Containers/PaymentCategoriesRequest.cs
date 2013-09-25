using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("PaymentCategoriesRequest")]
    [DataContract(Name = "PaymentCategoriesRequest", Namespace = "urn:CashIn")]
    public class PaymentCategoriesRequest : StandardRequest
    {
        private string _ProviderName;        

        [XmlElement(ElementName = "ProviderName")]
        [DataMember(Name = "ProviderName")]
        public string ProviderName
        {
            get { return _ProviderName; }
            set { _ProviderName = value; }
        }

        public PaymentCategoriesRequest()
        {
        }

        public PaymentCategoriesRequest(int terminalId)
            : base(terminalId)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, ProviderName: {1}", base.ToString(), _ProviderName);
        }
    }
}
