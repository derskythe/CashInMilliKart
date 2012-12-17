using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Currency")]
    [DataContract(Name = "Currency", Namespace = "urn:CashIn")]
    public class Currency
    {
        private string _Id;
        private string _IsoName;
        private string _Name;
        private bool _DefaultCurrency;
        private decimal _CurrencyRate;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "IsoName")]
        [DataMember(Name = "IsoName")]
        public string IsoName
        {
            get { return _IsoName; }
            set { _IsoName = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "DefaultCurrency")]
        [DataMember(Name = "DefaultCurrency")]
        public bool DefaultCurrency
        {
            get { return _DefaultCurrency; }
            set { _DefaultCurrency = value; }
        }

        [XmlElement(ElementName = "CurrencyRate")]
        [DataMember(Name = "CurrencyRate")]
        public decimal CurrencyRate
        {
            get { return _CurrencyRate; }
            set { _CurrencyRate = value; }
        }

        public Currency()
        {
        }

        public Currency(string id, string isoName, string name, bool defaultCurrency, decimal currencyRate)
        {
            _Id = id;
            _IsoName = isoName;
            _Name = name;
            _DefaultCurrency = defaultCurrency;
            _CurrencyRate = currencyRate;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, IsoName: {1}, Name: {2}, DefaultCurrency: {3}, CurrencyRate: {4}", _Id,
                                 _IsoName, _Name, _DefaultCurrency, _CurrencyRate);
        }
    }
}
