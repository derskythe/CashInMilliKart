using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("SaveTerminalResult")]
    [DataContract(Name = "SaveTerminalResult", Namespace = "urn:CashIn")]
    public class SaveTerminalResult : StandardResult
    {
        private String _Mac;

        [XmlElement(ElementName = "Mac")]
        [DataMember(Name = "Mac")]
        public string Mac
        {
            get { return _Mac; }
            set { _Mac = value; }
        }

        public SaveTerminalResult()
        {
        }

        public SaveTerminalResult(ResultCodes code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Mac: {1}", base.ToString(), _Mac);
        }
    }
}
