using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Containers
{
    [Serializable, XmlRoot("Terminal")]
    [DataContract(Name = "Terminal", Namespace = "urn:CashIn")]
    public class Terminal
    {
        private int _Id;
        private string _Name;
        private string _Address;
        private string _IdentityName;
        private byte[] _SignKey;
        private string _Ip;
        private byte[] _TmpKey;
        private int _LastStatusType;
        private int _LastCashcodeStatus;
        private DateTime _LastStatusUpdate;

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

        [XmlElement(ElementName = "SignKey")]
        [DataMember(Name = "SignKey")]
        public byte[] SignKey
        {
            get { return _SignKey; }
            set { _SignKey = value; }
        }

        [XmlElement(ElementName = "Ip")]
        [DataMember(Name = "Ip")]
        public string Ip
        {
            get { return _Ip; }
            set { _Ip = value; }
        }

        [XmlElement(ElementName = "TmpKey")]
        [DataMember(Name = "TmpKey")]
        public byte[] TmpKey
        {
            get { return _TmpKey; }
            set { _TmpKey = value; }
        }

        [XmlElement(ElementName = "LastStatusType")]
        [DataMember(Name = "LastStatusType")]
        public int LastStatusType
        {
            get { return _LastStatusType; }
            set { _LastStatusType = value; }
        }

        [XmlElement(ElementName = "LastCashcodeStatus")]
        [DataMember(Name = "LastCashcodeStatus")]
        public int LastCashcodeStatus
        {
            get { return _LastCashcodeStatus; }
            set { _LastCashcodeStatus = value; }
        }

        [XmlElement(ElementName = "LastStatusUpdate")]
        [DataMember(Name = "LastStatusUpdate")]
        public DateTime LastStatusUpdate
        {
            get { return _LastStatusUpdate; }
            set { _LastStatusUpdate = value; }
        }

        public Terminal()
        {
        }

        public Terminal(int id, string name, string address, string identityName, byte[] signKey, string ip, byte[] tmpKey, int lastStatusType, int lastCashcodeStatus, DateTime lastStatusUpdate)
        {
            _Id = id;
            _Name = name;
            _Address = address;
            _IdentityName = identityName;
            _SignKey = signKey;
            _Ip = ip;
            _TmpKey = tmpKey;
            _LastStatusType = lastStatusType;
            _LastCashcodeStatus = lastCashcodeStatus;
            _LastStatusUpdate = lastStatusUpdate;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Name: {1}, Address: {2}, IdentityName: {3}, SignKey: {4}, Ip: {5}, TmpKey: {6}, LastStatusType: {7}, LastCashcodeStatus: {8}, LastStatusUpdate: {9}",
                    _Id, _Name, _Address, _IdentityName, _SignKey, _Ip, _TmpKey, _LastStatusType, _LastCashcodeStatus,
                    _LastStatusUpdate);
        }
    }
}
