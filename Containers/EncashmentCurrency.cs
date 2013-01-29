using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("EncashmentCurrency")]
    [DataContract(Name = "EncashmentCurrency", Namespace = "urn:CashIn")]
    public class EncashmentCurrency
    {
        private string _Currency;
        private int _Amount;

        [XmlElement(ElementName = "Currency")]
        [DataMember(Name = "Currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public EncashmentCurrency()
        {
        }

        public EncashmentCurrency(string currency, int amount)
        {
            _Currency = currency;
            _Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("Currency: {0}, Amount: {1}", _Currency, _Amount);
        }
    }
}
