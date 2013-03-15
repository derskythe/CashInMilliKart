using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("CheckField")]
    [DataContract(Name = "CheckField", Namespace = "urn:CashIn")]
    public class CheckField
    {
        private int _Id;
        private int _CheckId;
        private byte[] _Image;
        private string _Value;
        private int _FieldType;
        private int _Order;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "CheckId")]
        [DataMember(Name = "CheckId")]
        public int CheckId
        {
            get { return _CheckId; }
            set { _CheckId = value; }
        }

        [XmlElement(ElementName = "Image")]
        [DataMember(Name = "Image")]
        public byte[] Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        [XmlElement(ElementName = "Value")]
        [DataMember(Name = "Value")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        [XmlElement(ElementName = "FieldType")]
        [DataMember(Name = "FieldType")]
        public int FieldType
        {
            get { return _FieldType; }
            set { _FieldType = value; }
        }

        [XmlElement(ElementName = "Order")]
        [DataMember(Name = "Order")]
        public int Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public CheckField()
        {
        }

        public CheckField(int id, int checkId, byte[] image, string value, int fieldType, int order)
        {
            _Id = id;
            _CheckId = checkId;
            _Image = image;
            _Value = value;
            _FieldType = fieldType;
            _Order = order;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, CheckId: {1}, Image: {2}, Value: {3}, FieldType: {4}, Order: {5}", _Id,
                                 _CheckId, _Image, _Value, _FieldType, _Order);
        }
    }
}
