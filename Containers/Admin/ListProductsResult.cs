using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.Admin
{
    [Serializable, XmlRoot("ListProductsResult")]
    [DataContract(Name = "ListProductsResult", Namespace = "urn:CashIn")]
    public class ListProductsResult : StandardResult
    {
        private IEnumerable<Product> _Products;

        public ListProductsResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Products")]
        [DataMember(Name = "Products")]
        public IEnumerable<Product> Products
        {
            get { return _Products; }
            set { _Products = value; }
        }

        public ListProductsResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Products: {1}", base.ToString(), EnumEx.GetStringFromArray(_Products));
        }
    }
}
