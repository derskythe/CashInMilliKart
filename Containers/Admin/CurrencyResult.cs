using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("EncashmentResult")]
    [DataContract(Name = "EncashmentResult", Namespace = "urn:CashIn")]
    public class CurrencyResult : StandardResult
    {
        private Currency _Currency;

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public Currency Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        public CurrencyResult()
        {
        }

        public CurrencyResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Currency: {1}", base.ToString(), _Currency);
        }
    }
}
