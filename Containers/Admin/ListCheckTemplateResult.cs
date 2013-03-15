using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListCheckTemplateResult")]
    [DataContract(Name = "ListCheckTemplateResult", Namespace = "urn:CashIn")]
    public class ListCheckTemplateResult : StandardResult
    {
        private List<CheckTemplate> _Templates;

        [XmlArray("Templates")]
        [DataMember(Name = "Templates")]
        public List<CheckTemplate> Templates
        {
            get { return _Templates; }
            set { _Templates = value; }
        }

        public ListCheckTemplateResult()
        {
        }

        public ListCheckTemplateResult(ResultCodes code) : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Templates: {1}", base.ToString(), EnumEx.GetStringFromArray(_Templates));
        }
    }
}
