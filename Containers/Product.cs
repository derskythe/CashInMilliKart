﻿using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Product")]
    [DataContract(Name = "Product", Namespace = "urn:CashIn")]
    public class Product
    {
        private decimal _Id;
        private string _Name;
        private string _NameAz;
        private string _NameRu;
        private string _NameEn;
        private string _Assembly;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public decimal Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "NameAz")]
        [DataMember(Name = "NameAz")]
        public string NameAz
        {
            get { return _NameAz; }
            set { _NameAz = value; }
        }

        [XmlElement(ElementName = "NameRu")]
        [DataMember(Name = "NameRu")]
        public string NameRu
        {
            get { return _NameRu; }
            set { _NameRu = value; }
        }

        [XmlElement(ElementName = "NameEn")]
        [DataMember(Name = "NameEn")]
        public string NameEn
        {
            get { return _NameEn; }
            set { _NameEn = value; }
        }

        [XmlElement(ElementName = "Assembly")]
        [DataMember(Name = "Assembly")]
        public string Assembly
        {
            get { return _Assembly; }
            set { _Assembly = value; }
        }

        public Product()
        {
        }

        public Product(decimal id, string name, string nameAz, string nameRu, string nameEn, string assembly)
        {
            _Id = id;
            _Name = name;
            _NameAz = nameAz;
            _NameRu = nameRu;
            _NameEn = nameEn;
            _Assembly = assembly;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, NameAz: {2}, NameRu: {3}, NameEn: {4}, Assembly: {5}", _Id, _Name,
                                 _NameAz, _NameRu, _NameEn, _Assembly);
        }
    }
}
