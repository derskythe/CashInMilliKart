using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("BaseMessage")]
    [DataContract(Name = "BaseMessage", Namespace = "urn:CashIn")]
    public abstract class BaseMessage
    {
        protected String _Sign;
        protected DateTime _SystemTime;

        [XmlElement(ElementName = "SystemTime")]
        [DataMember(Name = "SystemTime")]
        public DateTime SystemTime
        {
            get { return _SystemTime; }
            set { _SystemTime = value; }
        }

        [XmlElement(ElementName = "Sign")]
        [DataMember(Name = "Sign")]
        public string Sign
        {
            get { return _Sign; }
            set { _Sign = value; }
        }

        protected BaseMessage()
        {
            _SystemTime = DateTime.Now;
        }

        public override string ToString()
        {
            return String.Empty;
        }
    }
}
