using System;
using System.ServiceModel;
using System.Text;
using CashInCore.Properties;
using Containers;
using Containers.Enums;
using Db;
using NLog;
using crypto;

namespace CashInCore
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerSession,
        UseSynchronizationContext = true,
        ConfigurationName = "CashInCore.CashInServer")]
    public partial class CashInServer : ICashInServer
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        static CashInServer()
        {
            Log.Debug("Constructor");
            try
            {
                if (String.IsNullOrEmpty(Settings.Default.PrivateKey) ||
                    String.IsNullOrEmpty(Settings.Default.PublicKey))
                {
                    var keys = Wrapper.SaveToString(Wrapper.GenerateKeys(1024));
                    Settings.Default.PrivateKey = keys[0];
                    Settings.Default.PublicKey = keys[1];

                    Settings.Default.Save();
                }

                GetPrivateKey();
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public string GetPublicKey()
        {
            return Settings.Default.PublicKey;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public AuthResult InitTerminal(int terminalId, string authKey, string publicKey)
        {
            Log.Info("InitTerminal. terminalId: {0}, authKey: {1}, publicKey: {2}");
            var result = new AuthResult();

            try
            {
                if (String.IsNullOrEmpty(publicKey))
                {
                    result.Code = ResultCodes.InvalidKey;
                    throw new Exception("Codes not equal");
                }

                if (String.IsNullOrEmpty(authKey))
                {
                    result.Code = ResultCodes.InvalidParameters;
                    throw new Exception("Auth code is NULL!");
                }

                var terminal = OracleDb.Instance.GetTerminal(terminalId);

                if (terminal == null)
                {
                    result.Code = ResultCodes.InvalidTerminal;
                    throw new Exception("Terminal not found in DB");
                }

                if (terminal.TmpKey == null)
                {
                    result.Code = ResultCodes.InvalidTerminal;
                    throw new Exception(String.Format("Code is empty. TerminalId: {0}", terminalId));
                }

                if (Encoding.ASCII.GetString(terminal.TmpKey) != authKey)
                {
                    result.Code = ResultCodes.InvalidTerminal;
                    throw new Exception(String.Format("Codes not equal. TerminalKey: {0}, Db key: {1}", Encoding.ASCII.GetString(terminal.TmpKey), authKey));
                }

                OracleDb.Instance.SaveTerminalStatus(terminalId, (int)TerminalCodes.Ok, 0);

                var terminalKey = Encoding.ASCII.GetBytes(publicKey);
                OracleDb.Instance.SaveTerminalKey(terminalId, Encoding.ASCII.GetBytes(publicKey));

                result.Code = ResultCodes.Ok;
                result.PublicKey = Settings.Default.PublicKey;
                result.Sign = DoSign(terminalId, result.SystemTime, terminalKey);

                Log.Info("Init terminal " + terminal.Id + " Terminal info:\n" + terminal);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public PingResult Ping(PingRequest request)
        {
            Log.Debug("Ping");
            var result = new PingResult();

            try
            {
                Terminal terminalInfo;
                Log.Debug(request);
                result = (PingResult)AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.SaveTerminalStatus(request.TerminalId, request.TerminalStatus, (int)request.CashCodeStatus.StateCodeOut);
                result.Command = OracleDb.Instance.GetTerminalStatus(request.TerminalId);
                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult CommandReceived(StandardRequest request)
        {
            Log.Debug("CommandReceived");
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.TerminalSetDone(request.TerminalId);

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public TerminalInfoResult GetTerminalInfo(StandardRequest request)
        {
            Log.Info("GetTerminalInfo");

            var result = new TerminalInfoResult();

            try
            {
                Terminal terminalInfo;
                result = (TerminalInfoResult)AuthTerminal(result, request, out terminalInfo);


                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);

                terminalInfo.SignKey = new byte[0];
                terminalInfo.TmpKey = new byte[0];

                result.Terminal = terminalInfo;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ProductResult ListProducts(StandardRequest request)
        {
            Log.Info("ListProducts");

            var result = new ProductResult();

            try
            {
                Terminal terminalInfo;
                result = (ProductResult)AuthTerminal(result, request, out terminalInfo);

                var list = OracleDb.Instance.ListProducts(request.TerminalId);
                result.Products = list.ToArray();

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public CurrenciesResult ListCurrencies(StandardRequest request)
        {
            Log.Info("ListCurrencies");
            var result = new CurrenciesResult();

            try
            {
                Terminal terminalInfo;
                result = (CurrenciesResult)AuthTerminal(result, request, out terminalInfo);

                var list = OracleDb.Instance.ListCurrencies();
                result.Currencies = list.ToArray();

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult Payment(PaymentInfoByProducts request)
        {
            Log.Info("Payment");
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                Log.Debug("Payment: " + request);

                OracleDb.Instance.SavePayment(request);

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult Encashment(Encashment request)
        {
            Log.Info("Encashment");

            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.SaveEncashment(request);

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }
    }
}
