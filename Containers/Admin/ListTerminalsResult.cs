using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListTerminalsResult")]
    [DataContract(Name = "ListTerminalsResult", Namespace = "urn:CashIn")]
    public class ListTerminalsResult : StandardResult
    {
        private Terminal[] _Terminals;
        private int _Count;

        [XmlElement(ElementName = "Count")]
        [DataMember(Name = "Count")]
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        public ListTerminalsResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Terminals")]
        [DataMember(Name = "Terminals")]
        public Terminal[] Terminals
        {
            get { return _Terminals; }
            set { _Terminals = value; }
        }

        public ListTerminalsResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Terminals: {1}, Count: {2}", base.ToString(), EnumEx.GetStringFromArray(_Terminals), _Count);
        }
    }
}
