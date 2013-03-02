using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("EncashmentResult")]
    [DataContract(Name = "EncashmentResult", Namespace = "urn:CashIn")]
    public class EncashmentResult : StandardResult
    {
        private Encashment _Encashment;

        public EncashmentResult(ResultCodes code) : base(code)
        {
        }

        [XmlElement(ElementName = "Encashment")]
        [DataMember(Name = "Encashment")]
        public Encashment Encashment
        {
            get { return _Encashment; }
            set { _Encashment = value; }
        }

        public EncashmentResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Encashment: {1}", base.ToString(), _Encashment);
        }
    }
}
