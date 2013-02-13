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
        private IEnumerable<Encashment> _Encashments;
       
        [XmlArray("Encashments")]
        [DataMember(Name = "Encashments")]
        public IEnumerable<Encashment> Encashments
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
            return string.Format("{0}, Encashments: {1}", base.ToString(), EnumEx.GetStringFromArray(_Encashments));
        }
    }
}
