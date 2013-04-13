using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListBanknoteSummary")]
    [DataContract(Name = "ListBanknoteSummary", Namespace = "urn:CashIn")]
    public class ListBanknoteSummary : StandardResult
    {
        private List<BanknoteSummary> _Summaries;

        public ListBanknoteSummary(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Summaries")]
        [DataMember(Name = "Summaries")]
        public List<BanknoteSummary> Summaries
        {
            get { return _Summaries; }
            set { _Summaries = value; }
        }

        public ListBanknoteSummary()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Summaries: {1}", base.ToString(), EnumEx.GetStringFromArray(_Summaries));
        }
    }
}
