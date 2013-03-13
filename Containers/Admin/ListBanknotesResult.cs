using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListBanknotesResult")]
    [DataContract(Name = "ListBanknotesResult", Namespace = "urn:CashIn")]
    public class ListBanknotesResult : StandardResult
    {
        private List<Banknote> _Banknotes;

        [XmlArray("Banknotes")]
        [DataMember(Name = "Banknotes")]
        public List<Banknote> Banknotes
        {
            get { return _Banknotes; }
            set { _Banknotes = value; }
        }

        public ListBanknotesResult()
        {
        }

        public ListBanknotesResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Banknotes: {1}", base.ToString(), EnumEx.GetStringFromArray(_Banknotes));
        }
    }
}
