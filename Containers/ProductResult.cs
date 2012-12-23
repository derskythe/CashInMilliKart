using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers
{
    [Serializable, XmlRoot("ProductResult")]
    [DataContract(Name = "ProductResult", Namespace = "urn:CashIn")]
    public class ProductResult : StandardResult
    {
        private Product[] _Products;

        [XmlArray("Products")]
        [DataMember(Name = "Products")]
        public Product[] Products
        {
            get { return _Products; }
            set { _Products = value; }
        }

        public ProductResult()
        {
        }

        public ProductResult(ResultCodes code) : base(code)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Products: {1}", base.ToString(), EnumEx.ToStringArray(_Products));
        }
    }
}
