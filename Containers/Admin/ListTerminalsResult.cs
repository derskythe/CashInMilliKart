using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListTerminalsResult")]
    [DataContract(Name = "ListTerminalsResult", Namespace = "urn:CashIn")]
    public class ListTerminalsResult : StandardResult
    {
        private IEnumerable<Terminal> _Terminals;

        public ListTerminalsResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Terminals")]
        [DataMember(Name = "Terminals")]
        public IEnumerable<Terminal> Terminals
        {
            get { return _Terminals; }
            set { _Terminals = value; }
        }

        public ListTerminalsResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Terminals: {1}", base.ToString(), EnumEx.GetStringFromArray(_Terminals));
        }
    }
}
