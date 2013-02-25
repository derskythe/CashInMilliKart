using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListCurrenciesResult")]
    [DataContract(Name = "ListCurrenciesResult", Namespace = "urn:CashIn")]
    public class ListCurrenciesResult : StandardResult
    {
        private List<Currency> _Currencies;

        public ListCurrenciesResult(ResultCodes code)
            : base(code)
        {
        }

        [XmlArray("Currencies")]
        [DataMember(Name = "Currencies")]
        public List<Currency> Currencies
        {
            get { return _Currencies; }
            set { _Currencies = value; }
        }

        public ListCurrenciesResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Currencies: {1}", base.ToString(), EnumEx.GetStringFromArray(_Currencies));
        }
    }
}
