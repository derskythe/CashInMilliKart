using System;
using System.ServiceModel;
using System.Text;
using Containers;
using Containers.Enums;
using Db;
using NLog;
using Org.BouncyCastle.Utilities.Encoders;

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

        [OperationBehavior(AutoDisposeParameters = true)]
        public AuthResult InitTerminal(int terminalId, string authKey, string publicKey)
        {
            Log.Info("InitTerminal");
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

                if (Encoding.ASCII.GetString(terminal.TmpKey) != authKey)
                {
                    result.Code = ResultCodes.InvalidTerminal;
                    throw new Exception(String.Format("Codes not equal. TerminalKey: {0}, Db key: {1}", Encoding.ASCII.GetString(terminal.TmpKey), authKey));
                }

                OracleDb.Instance.UpdateTerminalStatus(terminalId, (int)TerminalCodes.Ok, 0);
                OracleDb.Instance.UpdateTerminalKey(terminalId, UrlBase64.Decode(publicKey));

                result.Code = ResultCodes.Ok;

                Log.Info("Init terminal " + terminal + " Terminal info:\n" + terminal);
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
                result = (PingResult)AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.UpdateTerminalStatus(request.TerminalId, request.TerminalStatus, request.CashCodeStatus);
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
