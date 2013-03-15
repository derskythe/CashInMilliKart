using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers
{
    [Serializable, XmlRoot("GetClientInfoResult")]
    [DataContract(Name = "GetClientInfoResult", Namespace = "urn:CashIn")]
    public class GetClientInfoResult : StandardResult
    {
        private List<ClientInfo> _Infos;

        public GetClientInfoResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Infos")]
        [DataMember(Name = "Infos")]
        public List<ClientInfo> Infos
        {
            get { return _Infos; }
            set { _Infos = value; }
        }

        public GetClientInfoResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Infos: {1}", base.ToString(), EnumEx.GetStringFromArray(_Infos));
        }
    }
}
