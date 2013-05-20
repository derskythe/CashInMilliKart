using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("TerminalVersionRequest")]
    [DataContract(Name = "TerminalVersionRequest", Namespace = "urn:CashIn")]
    public class TerminalVersionExtRequest : StandardRequest
    {
        private String _Version;
        private List<String> _AvailableCurrencies;
        private String _CashcodeVersion;

        public TerminalVersionExtRequest(int terminalId) : base(terminalId)
        {
        }

        [XmlElement(ElementName = "Version")]
        [DataMember(Name = "Version")]
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        [XmlElement(ElementName = "AvailableCurrencies")]
        [DataMember(Name = "AvailableCurrencies")]
        public List<string> AvailableCurrencies
        {
            get { return _AvailableCurrencies; }
            set { _AvailableCurrencies = value; }
        }

        [XmlElement(ElementName = "CashcodeVersion")]
        [DataMember(Name = "CashcodeVersion")]
        public string CashcodeVersion
        {
            get { return _CashcodeVersion; }
            set { _CashcodeVersion = value; }
        }

        public TerminalVersionExtRequest()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Version: {1}, AvailableCurrencies: {2}, CashcodeVersion: {3}", base.ToString(),
                                 _Version, EnumEx.GetStringFromArray(_AvailableCurrencies), _CashcodeVersion);
        }
    }
}
