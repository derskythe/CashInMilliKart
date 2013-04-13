using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

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
        private int _BillsCount;
        private DateTime _LastStatusUpdate;
        private int _LastPrinterStatus;
        private int _LastCashcodeError;
        private int _LastCashcodeOutStatus;
        private int _LastCashcodeSuberror;
        private int _LastPrinterErrorState;
        private int _LastPrinterExtErrorState;
        private int _LastEncashment;
        private MultiLanguageString _TerminalStatusDesc;
        private MultiLanguageString _CashcodeDesc;
        private MultiLanguageString _PrinterStatusDesc;
        private MultiLanguageString _PrinterErrorStatusDesc;
        private MultiLanguageString _PrinterExtErrorStatusDesc;
        private String _TerminalStatusName;
        private String _CashcodeStatusName;
        private String _PrinterStatusName;
        private String _PrinterErrorStatusName;
        private String _PrinterExtErrorStatusName;
        private int _TerminalStatusType;
        private int _CashcodeStatusType;
        private int _PrinterStatusType;
        private string _Version;
        private int _BranchId;
        private String _BranchName;

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

        [XmlElement(ElementName = "LastPrinterStatus")]
        [DataMember(Name = "LastPrinterStatus")]
        public int LastPrinterStatus
        {
            get { return _LastPrinterStatus; }
            set { _LastPrinterStatus = value; }
        }

        [XmlElement(ElementName = "LastCashcodeError")]
        [DataMember(Name = "LastCashcodeError")]
        public int LastCashcodeError
        {
            get { return _LastCashcodeError; }
            set { _LastCashcodeError = value; }
        }

        [XmlElement(ElementName = "LastCashcodeOutStatus")]
        [DataMember(Name = "LastCashcodeOutStatus")]
        public int LastCashcodeOutStatus
        {
            get { return _LastCashcodeOutStatus; }
            set { _LastCashcodeOutStatus = value; }
        }

        [XmlElement(ElementName = "LastCashcodeSuberror")]
        [DataMember(Name = "LastCashcodeSuberror")]
        public int LastCashcodeSuberror
        {
            get { return _LastCashcodeSuberror; }
            set { _LastCashcodeSuberror = value; }
        }

        [XmlElement(ElementName = "LastPrinterErrorState")]
        [DataMember(Name = "LastPrinterErrorState")]
        public int LastPrinterErrorState
        {
            get { return _LastPrinterErrorState; }
            set { _LastPrinterErrorState = value; }
        }

        [XmlElement(ElementName = "LastPrinterExtErrorState")]
        [DataMember(Name = "LastPrinterExtErrorState")]
        public int LastPrinterExtErrorState
        {
            get { return _LastPrinterExtErrorState; }
            set { _LastPrinterExtErrorState = value; }
        }

        [XmlElement(ElementName = "LastEncashment")]
        [DataMember(Name = "LastEncashment")]
        public int LastEncashment
        {
            get { return _LastEncashment; }
            set { _LastEncashment = value; }
        }

        [XmlElement(ElementName = "TerminalStatusDesc")]
        [DataMember(Name = "TerminalStatusDesc")]
        public MultiLanguageString TerminalStatusDesc
        {
            get { return _TerminalStatusDesc; }
            set { _TerminalStatusDesc = value; }
        }

        [XmlElement(ElementName = "CashcodeDesc")]
        [DataMember(Name = "CashcodeDesc")]
        public MultiLanguageString CashcodeDesc
        {
            get { return _CashcodeDesc; }
            set { _CashcodeDesc = value; }
        }

        [XmlElement(ElementName = "PrinterStatusDesc")]
        [DataMember(Name = "PrinterStatusDesc")]
        public MultiLanguageString PrinterStatusDesc
        {
            get { return _PrinterStatusDesc; }
            set { _PrinterStatusDesc = value; }
        }

        [XmlElement(ElementName = "PrinterErrorStatusDesc")]
        [DataMember(Name = "PrinterErrorStatusDesc")]
        public MultiLanguageString PrinterErrorStatusDesc
        {
            get { return _PrinterErrorStatusDesc; }
            set { _PrinterErrorStatusDesc = value; }
        }

        [XmlElement(ElementName = "PrinterExtErrorStatusDesc")]
        [DataMember(Name = "PrinterExtErrorStatusDesc")]
        public MultiLanguageString PrinterExtErrorStatusDesc
        {
            get { return _PrinterExtErrorStatusDesc; }
            set { _PrinterExtErrorStatusDesc = value; }
        }

        [XmlElement(ElementName = "BillsCount")]
        [DataMember(Name = "BillsCount")]
        public int BillsCount
        {
            get { return _BillsCount; }
            set { _BillsCount = value; }
        }

        [XmlElement(ElementName = "TerminalStatusName")]
        [DataMember(Name = "TerminalStatusName")]
        public string TerminalStatusName
        {
            get { return _TerminalStatusName; }
            set { _TerminalStatusName = value; }
        }

        [XmlElement(ElementName = "CashcodeStatusName")]
        [DataMember(Name = "CashcodeStatusName")]
        public string CashcodeStatusName
        {
            get { return _CashcodeStatusName; }
            set { _CashcodeStatusName = value; }
        }

        [XmlElement(ElementName = "PrinterStatusName")]
        [DataMember(Name = "PrinterStatusName")]
        public string PrinterStatusName
        {
            get { return _PrinterStatusName; }
            set { _PrinterStatusName = value; }
        }

        [XmlElement(ElementName = "PrinterErrorStatusName")]
        [DataMember(Name = "PrinterErrorStatusName")]
        public string PrinterErrorStatusName
        {
            get { return _PrinterErrorStatusName; }
            set { _PrinterErrorStatusName = value; }
        }

        [XmlElement(ElementName = "PrinterExtErrorStatusName")]
        [DataMember(Name = "PrinterExtErrorStatusName")]
        public string PrinterExtErrorStatusName
        {
            get { return _PrinterExtErrorStatusName; }
            set { _PrinterExtErrorStatusName = value; }
        }

        [XmlElement(ElementName = "TerminalStatusType")]
        [DataMember(Name = "TerminalStatusType")]
        public int TerminalStatusType
        {
            get { return _TerminalStatusType; }
            set { _TerminalStatusType = value; }
        }

        [XmlElement(ElementName = "CashcodeStatusType")]
        [DataMember(Name = "CashcodeStatusType")]
        public int CashcodeStatusType
        {
            get { return _CashcodeStatusType; }
            set { _CashcodeStatusType = value; }
        }

        [XmlElement(ElementName = "PrinterStatusType")]
        [DataMember(Name = "PrinterStatusType")]
        public int PrinterStatusType
        {
            get { return _PrinterStatusType; }
            set { _PrinterStatusType = value; }
        }

        [XmlElement(ElementName = "Version")]
        [DataMember(Name = "Version")]
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        
        [XmlElement(ElementName = "BranchId")]
        [DataMember(Name = "BranchId")]
        public int BranchId
        {
            get { return _BranchId; }
            set { _BranchId = value; }
        }

        [XmlElement(ElementName = "BranchName")]
        [DataMember(Name = "BranchName")]
        public string BranchName
        {
            get { return _BranchName; }
            set { _BranchName = value; }
        }

        public Terminal()
        {
        }

        public Terminal(int id, string name, string address, string identityName, byte[] signKey, string ip, byte[] tmpKey, int lastStatusType, int lastCashcodeStatus, int billsCount, DateTime lastStatusUpdate, int lastPrinterStatus, int lastCashcodeError, int lastCashcodeOutStatus, int lastCashcodeSuberror, int lastPrinterErrorState, int lastPrinterExtErrorState, int lastEncashment, MultiLanguageString terminalStatusDesc, MultiLanguageString cashcodeDesc, MultiLanguageString printerStatusDesc, MultiLanguageString printerErrorStatusDesc, MultiLanguageString printerExtErrorStatusDesc, string terminalStatusName, string cashcodeStatusName, string printerStatusName, string printerErrorStatusName, string printerExtErrorStatusName, int terminalStatusType, int cashcodeStatusType, int printerStatusType, string version, int branchId, string branchName)
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
            _BillsCount = billsCount;
            _LastStatusUpdate = lastStatusUpdate;
            _LastPrinterStatus = lastPrinterStatus;
            _LastCashcodeError = lastCashcodeError;
            _LastCashcodeOutStatus = lastCashcodeOutStatus;
            _LastCashcodeSuberror = lastCashcodeSuberror;
            _LastPrinterErrorState = lastPrinterErrorState;
            _LastPrinterExtErrorState = lastPrinterExtErrorState;
            _LastEncashment = lastEncashment;
            _TerminalStatusDesc = terminalStatusDesc;
            _CashcodeDesc = cashcodeDesc;
            _PrinterStatusDesc = printerStatusDesc;
            _PrinterErrorStatusDesc = printerErrorStatusDesc;
            _PrinterExtErrorStatusDesc = printerExtErrorStatusDesc;
            _TerminalStatusName = terminalStatusName;
            _CashcodeStatusName = cashcodeStatusName;
            _PrinterStatusName = printerStatusName;
            _PrinterErrorStatusName = printerErrorStatusName;
            _PrinterExtErrorStatusName = printerExtErrorStatusName;
            _TerminalStatusType = terminalStatusType;
            _CashcodeStatusType = cashcodeStatusType;
            _PrinterStatusType = printerStatusType;
            _Version = version;
            _BranchId = branchId;
            _BranchName = branchName;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, Name: {1}, Address: {2}, IdentityName: {3}, Ip: {4}, LastStatusType: {5}, LastCashcodeStatus: {6}, BillsCount: {7}, LastStatusUpdate: {8}, LastPrinterStatus: {9}, LastCashcodeError: {10}, LastCashcodeOutStatus: {11}, LastCashcodeSuberror: {12}, LastPrinterErrorState: {13}, LastPrinterExtErrorState: {14}, TerminalStatusDesc: {15}, CashcodeDesc: {16}, PrinterStatusDesc: {17}, PrinterErrorStatusDesc: {18}, PrinterExtErrorStatusDesc: {19}, TerminalStatusName: {20}, CashcodeStatusName: {21}, PrinterStatusName: {22}, PrinterErrorStatusName: {23}, PrinterExtErrorStatusName: {24}, TerminalStatusType: {25}, CashcodeStatusType: {26}, PrinterStatusType: {27}, Version: {28}, BranchId: {29}, BranchName: {30}",
                    _Id, _Name, _Address, _IdentityName, _Ip, _LastStatusType, _LastCashcodeStatus, _BillsCount,
                    _LastStatusUpdate, _LastPrinterStatus, _LastCashcodeError, _LastCashcodeOutStatus,
                    _LastCashcodeSuberror, _LastPrinterErrorState, _LastPrinterExtErrorState, _TerminalStatusDesc,
                    _CashcodeDesc, _PrinterStatusDesc, _PrinterErrorStatusDesc, _PrinterExtErrorStatusDesc,
                    _TerminalStatusName, _CashcodeStatusName, _PrinterStatusName, _PrinterErrorStatusName,
                    _PrinterExtErrorStatusName, _TerminalStatusType, _CashcodeStatusType, _PrinterStatusType, _Version,
                    _BranchId, _BranchName);
        }
    }
}
