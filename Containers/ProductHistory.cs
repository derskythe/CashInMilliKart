using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("ProductHistory")]
    [DataContract(Name = "ProductHistory", Namespace = "urn:CashIn")]
    public class ProductHistory
    {
        private decimal _Id;
        private string _TransactionId;
        private decimal _TerminalId;
        private string _Name;
        private string _Address;
        private string _IdentityName;
        private decimal _ProductId;
        private string _CurrencyId;
        private decimal _Rate;
        private DateTime _InsertDate;
        private decimal _Amount;
        private DateTime _TerminalDate;
        private string _ProductName;
        private string _NameAz;
        private string _NameRu;
        private string _NameEn;
        private List<ProductHistoryValue> _Values;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public decimal Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "TransactionId")]
        [DataMember(Name = "TransactionId")]
        public string TransactionId
        {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        [XmlElement(ElementName = "TerminalId")]
        [DataMember(Name = "TerminalId")]
        public decimal TerminalId
        {
            get { return _TerminalId; }
            set { _TerminalId = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlElement(ElementName = "Address")]
        [DataMember(Name = "Address")]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        [XmlElement(ElementName = "IdentityName")]
        [DataMember(Name = "IdentityName")]
        public string IdentityName
        {
            get { return _IdentityName; }
            set { _IdentityName = value; }
        }

        [XmlElement(ElementName = "ProductId")]
        [DataMember(Name = "ProductId")]
        public decimal ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }

        [XmlElement(ElementName = "CurrencyId")]
        [DataMember(Name = "CurrencyId")]
        public string CurrencyId
        {
            get { return _CurrencyId; }
            set { _CurrencyId = value; }
        }

        [XmlElement(ElementName = "Rate")]
        [DataMember(Name = "Rate")]
        public decimal Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }

        [XmlElement(ElementName = "InsertDate")]
        [DataMember(Name = "InsertDate")]
        public DateTime InsertDate
        {
            get { return _InsertDate; }
            set { _InsertDate = value; }
        }

        [XmlElement(ElementName = "Amount")]
        [DataMember(Name = "Amount")]
        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [XmlElement(ElementName = "TerminalDate")]
        [DataMember(Name = "TerminalDate")]
        public DateTime TerminalDate
        {
            get { return _TerminalDate; }
            set { _TerminalDate = value; }
        }

        [XmlElement(ElementName = "ProductName")]
        [DataMember(Name = "ProductName")]
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
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

        [XmlArray("Values")]
        [DataMember(Name = "Values")]
        public List<ProductHistoryValue> Values
        {
            get { return _Values; }
            set { _Values = value; }
        }

        public ProductHistory()
        {
        }

        public ProductHistory(decimal id, string transactionId, decimal terminalId, string name, string address, string identityName, decimal productId, string currencyId, decimal rate, DateTime insertDate, decimal amount, DateTime terminalDate, string productName, string nameAz, string nameRu, string nameEn)
        {
            _Id = id;
            _TransactionId = transactionId;
            _TerminalId = terminalId;
            _Name = name;
            _Address = address;
            _IdentityName = identityName;
            _ProductId = productId;
            _CurrencyId = currencyId;
            _Rate = rate;
            _InsertDate = insertDate;
            _Amount = amount;
            _TerminalDate = terminalDate;
            _ProductName = productName;
            _NameAz = nameAz;
            _NameRu = nameRu;
            _NameEn = nameEn;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, TransactionId: {1}, TerminalId: {2}, Name: {3}, Address: {4}, IdentityName: {5}, ProductId: {6}, CurrencyId: {7}, Rate: {8}, InsertDate: {9}, Amount: {10}, TerminalDate: {11}, ProductName: {12}, NameAz: {13}, NameRu: {14}, NameEn: {15}, Values: {16}",
                    _Id, _TransactionId, _TerminalId, _Name, _Address, _IdentityName, _ProductId, _CurrencyId, _Rate,
                    _InsertDate, _Amount, _TerminalDate, _ProductName, _NameAz, _NameRu, _NameEn, EnumEx.GetStringFromArray(_Values));
        }
    }
}
