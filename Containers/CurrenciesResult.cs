using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers
{
    [Serializable, XmlRoot("CurrenciesResult")]
    [DataContract(Name = "CurrenciesResult", Namespace = "urn:CashIn")]
    public class CurrenciesResult : StandardResult
    {
        private Currency[] _Currencies;

        [XmlArray("Currencies")]
        [DataMember(Name = "Currencies")]
        public Currency[] Currencies
        {
            get { return _Currencies; }
            set { _Currencies = value; }
        }

        public CurrenciesResult()
        {
        }

        public CurrenciesResult(ResultCodes code)
            : base(code)
        {
        }


        public override string ToString()
        {
            return string.Format("{0}, Currencies: {1}", base.ToString(), _Currencies);
        }
    }
}
