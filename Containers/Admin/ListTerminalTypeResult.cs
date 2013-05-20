using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListTerminalTypeResult")]
    [DataContract(Name = "ListTerminalTypeResult", Namespace = "urn:CashIn")]
    public class ListTerminalTypeResult : StandardResult
    {
        private List<TerminalType> _TerminalTypes;

        public ListTerminalTypeResult(ResultCodes code)
            : base(code)
        {
        }

        [XmlArray("TerminalTypes")]
        [DataMember(Name = "TerminalTypes")]
        public List<TerminalType> TerminalTypes
        {
            get { return _TerminalTypes; }
            set { _TerminalTypes = value; }
        }

        public ListTerminalTypeResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, TerminalTypes: {1}", base.ToString(), EnumEx.GetStringFromArray(_TerminalTypes));
        }
    }
}
