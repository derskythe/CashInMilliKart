﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers
{
    [Serializable, XmlRoot("ProductHistory")]
    [DataContract(Name = "ProductHistory", Namespace = "urn:CashIn")]
    public class ProductHistory
    {
        private long _Id;
        private string _TransactionId;
        private long _TerminalId;
        private string _Name;
        private string _Address;
        private string _IdentityName;
        private long _ProductId;
        private long _EncashmentId;
        private string _CurrencyId;
        private float _Rate;
        private DateTime _InsertDate;
        private float _Amount;
        private DateTime _TerminalDate;
        private string _ProductName;
        private string _NameAz;
        private string _NameRu;
        private string _NameEn;
        private string _CreditNumber;
        private int _PaymentType;
        private MultiLanguageString _PaymentTypeName;
        private List<ProductHistoryValue> _Values;
        private List<BanknoteSummary> _Banknotes;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public long Id
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
        public long TerminalId
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
        public long ProductId
        {
            get { return _ProductId; }
            set { _ProductId = value; }
        }

        [XmlElement(ElementName = "EncashmentId")]
        [DataMember(Name = "EncashmentId")]
        public long EncashmentId
        {
            get { return _EncashmentId; }
            set { _EncashmentId = value; }
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
        public float Rate
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
        public float Amount
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

        [XmlElement(ElementName = "CreditNumber")]
        [DataMember(Name = "CreditNumber")]
        public string CreditNumber
        {
            get { return _CreditNumber; }
            set { _CreditNumber = value; }
        }

        [XmlElement(ElementName = "PaymentType")]
        [DataMember(Name = "PaymentType")]
        public int PaymentType
        {
            get { return _PaymentType; }
            set { _PaymentType = value; }
        }

        [XmlElement(ElementName = "PaymentTypeName")]
        [DataMember(Name = "PaymentTypeName")]
        public MultiLanguageString PaymentTypeName
        {
            get { return _PaymentTypeName; }
            set { _PaymentTypeName = value; }
        }

        [XmlArray("Values")]
        [DataMember(Name = "Values")]
        public List<ProductHistoryValue> Values
        {
            get { return _Values; }
            set { _Values = value; }
        }

        [XmlArray("Banknotes")]
        [DataMember(Name = "Banknotes")]
        public List<BanknoteSummary> Banknotes
        {
            get { return _Banknotes; }
            set { _Banknotes = value; }
        }

        public ProductHistory()
        {
        }

        public ProductHistory(long id, string transactionId, long terminalId, string name, string address, string identityName, long productId, long encashmentId, string currencyId, float rate, DateTime insertDate, float amount, DateTime terminalDate, string productName, string nameAz, string nameRu, string nameEn, string creditNumber, int paymentType, MultiLanguageString paymentTypeName, List<ProductHistoryValue> values, List<BanknoteSummary> banknotes)
        {
            _Id = id;
            _TransactionId = transactionId;
            _TerminalId = terminalId;
            _Name = name;
            _Address = address;
            _IdentityName = identityName;
            _ProductId = productId;
            _EncashmentId = encashmentId;
            _CurrencyId = currencyId;
            _Rate = rate;
            _InsertDate = insertDate;
            _Amount = amount;
            _TerminalDate = terminalDate;
            _ProductName = productName;
            _NameAz = nameAz;
            _NameRu = nameRu;
            _NameEn = nameEn;
            _CreditNumber = creditNumber;
            _PaymentType = paymentType;
            _PaymentTypeName = paymentTypeName;
            _Values = values;
            _Banknotes = banknotes;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, TransactionId: {1}, TerminalId: {2}, Name: {3}, Address: {4}, IdentityName: {5}, ProductId: {6}, EncashmentId: {7}, CurrencyId: {8}, Rate: {9}, InsertDate: {10}, Amount: {11}, TerminalDate: {12}, ProductName: {13}, NameAz: {14}, NameRu: {15}, NameEn: {16}, CreditNumber: {17}, PaymentType: {18}, PaymentTypeName: {19}, Values: {20}, Banknotes: {21}",
                    _Id, _TransactionId, _TerminalId, _Name, _Address, _IdentityName, _ProductId, _EncashmentId,
                    _CurrencyId, _Rate, _InsertDate, _Amount, _TerminalDate, _ProductName, _NameAz, _NameRu, _NameEn,
                    _CreditNumber, _PaymentType, _PaymentTypeName, EnumEx.GetStringFromArray(_Values),
                    EnumEx.GetStringFromArray(_Banknotes));
        }
    }
}
