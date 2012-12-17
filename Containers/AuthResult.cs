using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("AuthResult")]
    [DataContract(Name = "AuthResult", Namespace = "urn:CashIn")]
    public class AuthResult : StandardResult
    {
        private string _PublicKey;

        [XmlElement(ElementName = "PublicKey")]
        [DataMember(Name = "PublicKey")]
        public string PublicKey
        {
            get { return _PublicKey; }
            set { _PublicKey = value; }
        }

        public AuthResult()
        {
        }

        public AuthResult(ResultCodes code, string publicKey)
            : base(code)
        {
            _PublicKey = publicKey;
        }

        public override string ToString()
        {
            return string.Format("{0}, PublicKey: {1}", base.ToString(), _PublicKey);
        }
    }
}
