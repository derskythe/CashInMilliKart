using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers.MultiPayment
{
    [Serializable, XmlRoot("PaymentService")]
    [DataContract(Name = "PaymentService", Namespace = "urn:CashIn")]
    public class PaymentService
    {
        private int _Id;
        private string _Name;
        private MultiLanguageString _LocalizedName;
        private string _SubName;
        private string _PaypointPaymentType;
        private int _Type;
        private bool _FixedAmount;
        private int _CategoryId;
        private int _MinAmount;
        private int _MaxAmount;
        private string _AssemblyId;
        private List<PaymentFixedAmount> _AmountsList;
        private List<PaymentServiceField> _Fields;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
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

        [XmlElement(ElementName = "LocalizedName")]
        [DataMember(Name = "LocalizedName")]
        public MultiLanguageString LocalizedName
        {
            get { return _LocalizedName; }
            set { _LocalizedName = value; }
        }

        [XmlElement(ElementName = "SubName")]
        [DataMember(Name = "SubName")]
        public string SubName
        {
            get { return _SubName; }
            set { _SubName = value; }
        }

        [XmlElement(ElementName = "PaypointPaymentType")]
        [DataMember(Name = "PaypointPaymentType")]
        public string PaypointPaymentType
        {
            get { return _PaypointPaymentType; }
            set { _PaypointPaymentType = value; }
        }

        [XmlElement(ElementName = "Type")]
        [DataMember(Name = "Type")]
        public int Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        [XmlElement(ElementName = "FixedAmount")]
        [DataMember(Name = "FixedAmount")]
        public bool FixedAmount
        {
            get { return _FixedAmount; }
            set { _FixedAmount = value; }
        }

        [XmlElement(ElementName = "CategoryId")]
        [DataMember(Name = "CategoryId")]
        public int CategoryId
        {
            get { return _CategoryId; }
            set { _CategoryId = value; }
        }

        [XmlElement(ElementName = "MinAmount")]
        [DataMember(Name = "MinAmount")]
        public int MinAmount
        {
            get { return _MinAmount; }
            set { _MinAmount = value; }
        }

        [XmlElement(ElementName = "MaxAmount")]
        [DataMember(Name = "MaxAmount")]
        public int MaxAmount
        {
            get { return _MaxAmount; }
            set { _MaxAmount = value; }
        }

        [XmlElement(ElementName = "AssemblyId")]
        [DataMember(Name = "AssemblyId")]
        public string AssemblyId
        {
            get { return _AssemblyId; }
            set { _AssemblyId = value; }
        }

        [XmlArray("AmountsList")]
        [DataMember(Name = "AmountsList")]
        public List<PaymentFixedAmount> AmountsList
        {
            get { return _AmountsList; }
            set { _AmountsList = value; }
        }

        [XmlArray("Fields")]
        [DataMember(Name = "Fields")]
        public List<PaymentServiceField> Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public PaymentService()
        {
        }

        public PaymentService(int id, string name, MultiLanguageString localizedName, string subName, string paypointPaymentType, int type, bool fixedAmount, int categoryId, int minAmount, int maxAmount, string assemblyId, List<PaymentFixedAmount> amountsList, List<PaymentServiceField> fields)
        {
            _Id = id;
            _Name = name;
            _LocalizedName = localizedName;
            _SubName = subName;
            _PaypointPaymentType = paypointPaymentType;
            _Type = type;
            _FixedAmount = fixedAmount;
            _CategoryId = categoryId;
            _MinAmount = minAmount;
            _MaxAmount = maxAmount;
            _AssemblyId = assemblyId;
            _AmountsList = amountsList;
            _Fields = fields;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Name: {1}, LocalizedName: {2}, SubName: {3}, PaypointPaymentType: {4}, Type: {5}, FixedAmount: {6}, CategoryId: {7}, MinAmount: {8}, MaxAmount: {9}, AssemblyId: {10}, AmountsList: {11}, Fields: {12}",
                    _Id, _Name, _LocalizedName, _SubName, _PaypointPaymentType, _Type, _FixedAmount, _CategoryId,
                    _MinAmount, _MaxAmount, _AssemblyId, EnumEx.GetStringFromArray(_AmountsList),
                    EnumEx.GetStringFromArray(_Fields));
        }
    }
}
