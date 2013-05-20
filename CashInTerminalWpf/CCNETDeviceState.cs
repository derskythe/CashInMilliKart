using System.Collections.Generic;
using CashInTerminalWpf.CashIn;
using Containers.CashCode;

namespace CashInTerminalWpf
{
    public class CCNETDeviceState
    {
        private CCNETResponseStatus _StateCode; // внутр. состояние
        private CCNETRejectReason _RejectReason;
        private byte _SubStateCode; // подкод состояния
        private CCNETResponseStatus _StateCodeOut; // внеш. состояние
        private CCNETErrorCodes _ErrorCode;
        private bool _BillEnable; // мы включены и нормально работаем
        private bool _AcceptEnable; // Мы ждем денег
        private bool _FatalError;  // фатальная ошибка. устройство не может работать
        private int _Amount; // Сумма в купюроприемнике
        private int _WasAmount; // Было в купюроприемнике
        private int _Nominal; // номинал последней принятой купюры
        private string _DeviceStateDescription; // описание состояния
        private string _SubDeviceStateDescription; // дополнительное описание состояния
        private bool _Init; // Мы загружены
        private string _Currency;
        private bool _Stacking; // В режиме приема купюры.
        private string _Identification;
        private List<string> _AvailableCurrencies = new List<string>();

        /// <summary>
        /// Внутр. состояние
        /// </summary>
        public CCNETResponseStatus StateCode
        {
            get
            {
                return _StateCode;
            }
            set
            {
                _StateCode = value;
            }
        }

        /// <summary>
        /// Подкод состояния
        /// </summary>
        public byte SubStateCode
        {
            get
            {
                return _SubStateCode;
            }
            set
            {
                _SubStateCode = value;
            }
        }

        /// <summary>
        /// внеш. состояние
        /// </summary>
        public CCNETResponseStatus StateCodeOut
        {
            get
            {
                return _StateCodeOut;
            }
            set
            {
                _StateCodeOut = value;
            }
        }

        /// <summary>
        /// мы включены и нормально работаем
        /// </summary>
        public bool BillEnable
        {
            get
            {
                return _BillEnable;
            }
            set
            {
                _BillEnable = value;
            }
        }


        /// <summary>
        /// Мы ждем денег
        /// </summary>
        public bool AcceptEnable
        {
            get
            {
                return _AcceptEnable;
            }
            set
            {
                _AcceptEnable = value;
            }
        }


        /// <summary>
        /// фатальная ошибка. устройство не может работать
        /// </summary>
        public bool FatalError
        {
            get
            {
                return _FatalError;
            }
            set
            {
                _FatalError = value;
            }
        }


        /// <summary>
        /// Сумма в купюроприемнике
        /// </summary>
        public int Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }


        /// <summary>
        /// Было в купюроприемнике
        /// </summary>
        public int WasAmount
        {
            get
            {
                return _WasAmount;
            }
            set
            {
                _WasAmount = value;
            }
        }


        /// <summary>
        /// номинал последней принятой купюры
        /// </summary>
        public int Nominal
        {
            get
            {
                return _Nominal;
            }
            set
            {
                _Nominal = value;
            }
        }


        /// <summary>
        /// описание состояния
        /// </summary>
        public string DeviceStateDescription
        {
            get
            {
                return _DeviceStateDescription;
            }
            set
            {
                _DeviceStateDescription = value;
            }
        }

        /// <summary>
        /// дополнительное описание состояния
        /// </summary>
        public string SubDeviceStateDescription
        {
            get
            {
                return _SubDeviceStateDescription;
            }
            set
            {
                _SubDeviceStateDescription = value;
            }
        }

        /// <summary>
        /// В режиме приема купюры
        /// </summary>
        public bool Stacking
        {
            get
            {
                return _Stacking;
            }
            set
            {
                _Stacking = value;
            }
        }

        /// <summary>
        /// Мы загружены
        /// </summary>
        public bool Init
        {
            get
            {
                return _Init;
            }
            set
            {
                _Init = value;
            }
        }

        public string Identification
        {
            get { return _Identification; }
            set { _Identification = value; }
        }

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        public CCNETRejectReason RejectReason
        {
            get { return _RejectReason; }
            set { _RejectReason = value; }
        }

        public CCNETErrorCodes ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        public List<string> AvailableCurrencies
        {
            get { return _AvailableCurrencies; }
            set { _AvailableCurrencies = value; }
        }

        public CashCodeDeviceStatus ToCashCodeDeviceStatus()
        {
            var result = new CashCodeDeviceStatus
            {
                BillEnable = _BillEnable,
                DeviceStateDescription = _DeviceStateDescription,
                FatalError = _FatalError,
                Init = _Init,
                StateCode = (int)_StateCode,
                StateCodeOut = (int)_StateCodeOut,
                SubDeviceStateDescription = _SubDeviceStateDescription
            };

            return result;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "StateCode: {0}, RejectReason: {1}, SubStateCode: {2}, StateCodeOut: {3}, ErrorCode: {4}, BillEnable: {5}, AcceptEnable: {6}, FatalError: {7}, Amount: {8}, WasAmount: {9}, Nominal: {10}, DeviceStateDescription: {11}, SubDeviceStateDescription: {12}, Init: {13}, Currency: {14}, Stacking: {15}, Identification: {16}",
                    _StateCode, _RejectReason, _SubStateCode, _StateCodeOut, _ErrorCode, _BillEnable, _AcceptEnable,
                    _FatalError, _Amount, _WasAmount, _Nominal, _DeviceStateDescription, _SubDeviceStateDescription,
                    _Init, _Currency, _Stacking, _Identification);
        }
    }
}
