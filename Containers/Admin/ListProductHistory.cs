using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListProductHistory")]
    [DataContract(Name = "ListProductHistory", Namespace = "urn:CashIn")]
    public class ListProductHistory : StandardResult
    {
        
    }
}
