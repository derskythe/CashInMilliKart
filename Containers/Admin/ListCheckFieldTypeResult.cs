using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListCheckTemplateResult")]
    [DataContract(Name = "ListCheckTemplateResult", Namespace = "urn:CashIn")]
    public class ListCheckFieldTypeResult : StandardResult
    {
        private List<CheckFieldType> _Types;
       
        [XmlArray("Types")]
        [DataMember(Name = "Types")]
        public List<CheckFieldType> Types
        {
            get { return _Types; }
            set { _Types = value; }
        }

        public ListCheckFieldTypeResult()
        {
        }

        public ListCheckFieldTypeResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Types: {1}", base.ToString(), EnumEx.GetStringFromArray(_Types));
        }
    }
}