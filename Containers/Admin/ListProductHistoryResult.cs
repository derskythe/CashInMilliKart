using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListProductHistoryResult")]
    [DataContract(Name = "ListProductHistoryResult", Namespace = "urn:CashIn")]
    public class ListProductHistoryResult : StandardResult
    {
        private List<ProductHistory> _Histories;
        private int _Count;

        [XmlElement(ElementName = "Count")]
        [DataMember(Name = "Count")]
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        [XmlArray("Histories")]
        [DataMember(Name = "Histories")]
        public List<ProductHistory> Histories
        {
            get { return _Histories; }
            set { _Histories = value; }
        }

        public ListProductHistoryResult()
        {
        }

        public ListProductHistoryResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Histories: {1}, Count: {2}", base.ToString(), EnumEx.GetStringFromArray(_Histories), _Count);
        }
    }
}
