using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListCheckTypeResult")]
    [DataContract(Name = "ListCheckTypeResult", Namespace = "urn:CashIn")]
    public class ListCheckTypeResult : StandardResult
    {
        private List<CheckType> _Types;

        [XmlArray("Types")]
        [DataMember(Name = "Types")]
        public List<CheckType> Types
        {
            get { return _Types; }
            set { _Types = value; }
        }

        public ListCheckTypeResult()
        {
        }

        public ListCheckTypeResult(ResultCodes code) : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Types: {1}", base.ToString(), EnumEx.GetStringFromArray(_Types));
        }
    }
}
