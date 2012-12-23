using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers
{
    [Serializable, XmlRoot("PingResult")]
    [DataContract(Name = "PingResult", Namespace = "urn:CashIn")]
    public class PingResult : StandardResult
    {
        private int _Command;

        [XmlElement(ElementName = "Command")]
        [DataMember(Name = "Command")]
        public int Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        public PingResult() : base()
        {
        }

        public PingResult(ResultCodes code) : base(code)
        {
        }

        public PingResult(ResultCodes code, int command) : base(code)
        {
            _Command = command;
        }

        public override string ToString()
        {
            return string.Format("{0}, Command: {1}", base.ToString(), _Command);
        }
    }
}
