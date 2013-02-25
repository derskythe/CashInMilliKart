using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListCurrenciesResult")]
    [DataContract(Name = "ListCurrenciesResult", Namespace = "urn:CashIn")]
    public class ListTerminalCodesResult : StandardResult
    {

    }
}
