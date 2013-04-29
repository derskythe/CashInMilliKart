using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Enums;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("CategoriesResult")]
    [DataContract(Name = "CategoriesResult", Namespace = "urn:CashIn")]
    public class CategoriesResult : StandardResult
    {
        private List<PaymentCategory> _Categories;

        public CategoriesResult(ResultCodes code) : base(code)
        {
        }

        [XmlArray("Categories")]
        [DataMember(Name = "Categories")]
        public List<PaymentCategory> Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        public CategoriesResult()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Categories: {1}", base.ToString(), EnumEx.GetStringFromArray(_Categories));
        }
    }
}
