using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListCheckTemplateResult")]
    [DataContract(Name = "ListCheckTemplateResult", Namespace = "urn:CashIn")]
    public class CheckFieldResult : StandardResult
    {
        private List<CheckField> _Fields;

        public CheckFieldResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Fields")]
        [DataMember(Name = "Fields")]
        public List<CheckField> Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public CheckFieldResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Fields: {1}", base.ToString(), EnumEx.GetStringFromArray(_Fields));
        }
    }
}
