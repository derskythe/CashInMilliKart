using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListEncashmentResult")]
    [DataContract(Name = "ListEncashmentResult", Namespace = "urn:CashIn")]
    public class ListEncashmentResult : StandardResult
    {
        private List<Encashment> _Encashments;
        private int _Count;

        [XmlElement(ElementName = "Count")]
        [DataMember(Name = "Count")]
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        [XmlArray("Encashments")]
        [DataMember(Name = "Encashments")]
        public List<Encashment> Encashments
        {
            get { return _Encashments; }
            set { _Encashments = value; }
        }

        public ListEncashmentResult()
        {
        }

        public ListEncashmentResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Encashments: {1}, Count: {2}", base.ToString(), EnumEx.GetStringFromArray(_Encashments), _Count);
        }
    }
}
