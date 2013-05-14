using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using CashInCore.PaymentService;
using CashInCore.Properties;
using Containers;
using Containers.Admin;
using Containers.Enums;
using Containers.MultiPayment;
using Db;
using NLog;
using crypto;
using CategoriesResult = Containers.MultiPayment.CategoriesResult;

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
        private static String _MultiPaymentUsername;
        private static String _MultiPaymentPassword;

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

        public static void InitMultiPaymentService(String username, String password)
        {
            _MultiPaymentUsername = username;
            _MultiPaymentPassword = password;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public string GetPublicKey()
        {
            return Settings.Default.PublicKey;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public AuthResult InitTerminal(int terminalId, string authKey, string publicKey)
        {
            Log.Info(
                String.Format("InitTerminal. terminalId: {0}, authKey: {1}, publicKey: {2}",
                terminalId,
                authKey,
                publicKey
                ));
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
                    throw new Exception(String.Format("Codes not equal. TerminalKey: {0}, Db key: {1}",
                                                      Encoding.ASCII.GetString(terminal.TmpKey), authKey));
                }

                OracleDb.Instance.SaveTerminalStatus(terminalId, (int)TerminalCodes.Ok, 0, 0, 0, 0, 0, 0, 0, 0);

                var terminalKey = Encoding.ASCII.GetBytes(publicKey);
                OracleDb.Instance.SaveTerminalKey(terminalId, Encoding.ASCII.GetBytes(publicKey));
                OracleDb.Instance.RegisterIncassoOrder(terminalId);

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

                OracleDb.Instance.SaveTerminalStatus(
                    request.TerminalId,
                    request.TerminalStatus,
                    request.CashCodeStatus.StateCodeOut,
                    request.PrinterStatus.Status,
                    request.CashCodeStatus.ErrorCode,
                    request.CashCodeStatus.StateCodeOut,
                    request.CashCodeStatus.SubErrorCode,
                    request.PrinterStatus.ErrorState,
                    request.PrinterStatus.ExtendedErrorState,
                    request.CheckCount);
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
            Log.Debug("CommandReceived. " + request);
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.TerminalSetDone(request.TerminalId);
                OracleDb.Instance.SaveTerminalStatus(
                    request.TerminalId,
                    request.CommandResult);
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

                //var list = OracleDb.Instance.ListProducts(request.TerminalId);
                var list = OracleDb.Instance.ListProducts();
                Log.Debug(EnumEx.GetStringFromArray(list));
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
        [Obsolete]
        public StandardResult Payment(PaymentInfoByProducts request)
        {
            Log.Info("Payment. Data: " + request);
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                if (OracleDb.Instance.HasTransaction(request.TransactionId))
                {
                    result.Code = ResultCodes.Ok;
                    throw new InvalidDataException("Transaction already exists. TransactionId: " + request.TransactionId);
                }

                if (request.Amount <= 0)
                {
                    result.Code = ResultCodes.Ok;
                    throw new InvalidDataException("Invalid amount");
                }

                OracleDb.Instance.SavePayment(request);
                string bills = String.Join(";", request.Banknotes);
                OracleDb.Instance.CommitPayment(request.CreditNumber, request.Amount, bills, request.TerminalId,
                                                request.OperationType, request.TerminalDate, request.Currency);

                var paymentOperationType = (PaymentOperationType)request.OperationType;
                if (paymentOperationType == PaymentOperationType.GoldenPay ||
                    paymentOperationType == PaymentOperationType.Komtek)
                {
                    //var client = new PaymentServiceClient();

                    //var fields = new List<HistoryField>();
                    //foreach (var field in request.Info.Fields)
                    //{
                    //    var history = new HistoryField { name = field.Name, value = field.Value };

                    //    fields.Add(history);
                    //}

                    //var info = new PureInfo
                    //{
                    //    service_id = request.Info.ServiceId,
                    //    service_name = request.Info.ServiceName,
                    //    fields = fields.ToArray()
                    //};

                    //var serviceResult = client.GetInfoPure(_MultiPaymentUsername, _MultiPaymentPassword, info);
                }

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
        public StandardResult Pay(TerminalPaymentInfo request)
        {
            Log.Info("Pay. Data: " + request);
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                if (OracleDb.Instance.HasTransaction(request.TransactionId))
                {
                    result.Code = ResultCodes.Ok;
                    throw new InvalidDataException("Transaction already exists. TransactionId: " + request.TransactionId);
                }

                if (request.Amount <= 0)
                {
                    result.Code = ResultCodes.Ok;
                    throw new InvalidDataException("Invalid amount");
                }

                OracleDb.Instance.SavePaymentWithBackend(request);
                //string bills = String.Join(";", request.Banknotes);
                //OracleDb.Instance.CommitPayment(request.CreditNumber, request.Amount, bills, request.TerminalId,
                //                                request.OperationType, request.TerminalDate, request.Currency);

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);

                var paymentOperationType = (PaymentOperationType)request.OperationType;
                if (paymentOperationType == PaymentOperationType.GoldenPay ||
                    paymentOperationType == PaymentOperationType.Komtek)
                {
                    if (String.IsNullOrEmpty(_MultiPaymentUsername) || String.IsNullOrEmpty(_MultiPaymentPassword))
                    {
                        throw new Exception("MultiPaymentService not initiated!");
                    }

                    var client = new PaymentServiceClient();
                    var servicesResult = client.GetService(_MultiPaymentUsername, _MultiPaymentPassword,
                                                    request.PaymentServiceId);

                    if (servicesResult.code != ErrorCodes.Ok)
                    {
                        throw new Exception(servicesResult.description);
                    }

                    if (servicesResult.services == null || servicesResult.services.Length == 0)
                    {
                        throw new Exception("Service is null");
                    }
                    var service = servicesResult.services[0];
                    var fields = new List<HistoryField>();
                    int i = 0;
                    foreach (var field in service.service_fields)
                    {
                        var history = new HistoryField
                            {
                                name = field.name,
                                value = !String.IsNullOrEmpty(field.default_value) ? field.default_value : request.Values[i]
                            };

                        fields.Add(history);
                        i++;
                    }


                    var info = new PurePaymentInfo
                        {
                            amount = request.Amount * 100,
                            currency = "AZN",
                            payment_id = request.TransactionId,
                            service_id = service.id,
                            service_name = service.name,
                            fields = fields.ToArray()
                        };

                    Log.Debug(info);

                    var serviceResult = client.InsertPaymentPure(_MultiPaymentUsername, _MultiPaymentPassword, "CashIn", info);
                    if (serviceResult.code != ErrorCodes.Ok)
                    {
                        throw new Exception(serviceResult.description);
                    }
                }
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult UpdateTerminalVersion(TerminalVersionRequest request)
        {
            Log.Info("UpdateTerminalVersion. " + request);
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.SaveTerminalVersion(request.TerminalId, request.Version);
                result.Code = ResultCodes.Ok;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public StandardResult TerminalRestarted(StandardRequest request)
        {
            Log.Info("TerminalRestarted. " + request);
            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                var userId = OracleDb.Instance.GetLastTerminalCommandUserId(request.TerminalId,
                                                                            (int)TerminalCommands.Restart);
                OracleDb.Instance.SetTerminalStatusCode(userId, request.TerminalId, (int)TerminalCommands.NormalMode);

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
            Log.Info("Encashment. " + request);

            var result = new StandardResult();

            try
            {
                Terminal terminalInfo;
                result = AuthTerminal(result, request, out terminalInfo);

                OracleDb.Instance.SaveEncashment(request);
                var userId = OracleDb.Instance.GetLastTerminalCommandUserId(request.TerminalId,
                                                                            (int)TerminalCommands.Encash);
                Log.Debug("Encashment userID: " + userId);
                OracleDb.Instance.SetTerminalStatusCode(userId, request.TerminalId, (int)TerminalCommands.NormalMode);

                OracleDb.Instance.RegisterIncassoOrder(terminalInfo.Id);

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
        public GetClientInfoResult GetClientInfo(GetClientInfoRequest request)
        {
            Log.Info("GetClientInfo. " + request);
            var result = new GetClientInfoResult();

            try
            {
                Terminal terminalInfo;
                result = (GetClientInfoResult)AuthTerminal(result, request, out terminalInfo);

                switch ((PaymentOperationType)request.PaymentOperationType)
                {
                    case PaymentOperationType.CreditPaymentByClientCode:
                        result.Infos = OracleDb.Instance.ListCreditClients(request.ClientCode);
                        break;

                    case PaymentOperationType.CreditPaymentByPassportAndAccount:
                        result.Infos = OracleDb.Instance.ListCreditClients(request.CreditAccount, request.PasportNumber);
                        break;

                    case PaymentOperationType.CreditPaymentBolcard:
                        result.Infos = OracleDb.Instance.ListClientsBolcard(request.Bolcard8Digits);
                        break;

                    case PaymentOperationType.DebitPaymentByClientCode:
                        result.Infos = OracleDb.Instance.ListDebitClients(request.ClientCode);
                        break;

                    case PaymentOperationType.DebitPaymentByPassportAndAccount:
                        result.Infos = OracleDb.Instance.ListDebitClients(request.CreditAccount, request.PasportNumber);
                        break;
                }

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckTemplateResult ListCheckTemplateDigest(StandardRequest request)
        {
            Log.Info("ListCheckTemplateDigest. " + request);
            var result = new ListCheckTemplateResult();

            try
            {
                Terminal terminalInfo;
                result = (ListCheckTemplateResult)AuthTerminal(result, request, out terminalInfo);

                result.Templates = OracleDb.Instance.ListCheckTemplateDigest();

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public ListCheckTemplateResult ListCheckTemplate(StandardRequest request)
        {
            Log.Info("ListCheckTemplate. " + request);
            var result = new ListCheckTemplateResult();

            try
            {
                Terminal terminalInfo;
                result = (ListCheckTemplateResult)AuthTerminal(result, request, out terminalInfo);

                result.Templates = OracleDb.Instance.ListCheckTemplate();

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public BonusResponse GetBonusAmount(BonusRequest request)
        {
            Log.Debug("GetBonusAmount. Request: " + request);

            var result = new BonusResponse();

            try
            {
                Terminal terminalInfo;
                result = (BonusResponse)AuthTerminal(result, request, out terminalInfo);

                result.Bonus = OracleDb.Instance.GetBonusAmount(
                    request.CreditNumber,
                    request.Amount,
                    request.Currency);

                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public PaymentServiceInfoResponse GetPaymentServiceInfo(PaymentServiceInfoRequest request)
        {
            Log.Debug("GetPaymentServiceInfo. Request: " + request);

            var result = new PaymentServiceInfoResponse();

            try
            {
                Terminal terminalInfo;
                result = (PaymentServiceInfoResponse)AuthTerminal(result, request, out terminalInfo);

                var client = new PaymentServiceClient();


                var fields = new List<HistoryField>();
                foreach (var field in request.Info.Fields)
                {
                    var history = new HistoryField { name = field.Name, value = field.Value };

                    fields.Add(history);
                }

                var info = new PureInfo
                    {
                        service_id = request.Info.ServiceId,
                        service_name = request.Info.ServiceName,
                        fields = fields.ToArray()
                    };

                var serviceResult = client.GetInfoPure(_MultiPaymentUsername, _MultiPaymentPassword, info);
                Log.Debug(serviceResult.code);
                if (serviceResult.code != ErrorCodes.Ok && serviceResult.code != ErrorCodes.InvalidParameters)
                {
                    throw new Exception(serviceResult.description);
                }

                if (serviceResult.service_info != null)
                {
                    result.Person = serviceResult.service_info.person;
                    result.Debt = serviceResult.service_info.debt;
                }
                result.Code = serviceResult.code == ErrorCodes.InvalidParameters ? ResultCodes.InvalidNumber : ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public CategoriesResult ListPaymentCategories(StandardRequest request)
        {
            Log.Debug("ListPaymentCategories. Request: " + request);

            var result = new CategoriesResult();

            try
            {
                Terminal terminalInfo;
                result = (CategoriesResult)AuthTerminal(result, request, out terminalInfo);

                if (String.IsNullOrEmpty(_MultiPaymentUsername) || String.IsNullOrEmpty(_MultiPaymentPassword))
                {
                    throw new Exception("MultiPaymentService not initiated!");
                }

                var client = new PaymentServiceClient();
                var categories = client.ListCategories(_MultiPaymentUsername, _MultiPaymentPassword);

                if (categories.code != ErrorCodes.Ok)
                {
                    throw new Exception(categories.description);
                }


                var resultList = new List<Containers.MultiPayment.PaymentCategory>();
                foreach (var category in categories.Categories)
                {
                    var serviceList = new List<Containers.MultiPayment.PaymentService>();
                    if (category.services != null)
                    {
                        foreach (var service in category.services)
                        {
                            var serviceFieldList = new List<PaymentServiceField>();
                            if (service.service_fields != null)
                            {
                                foreach (var serviceField in service.service_fields)
                                {
                                    var amountFieldList = new List<PaymentServiceEnum>();
                                    if (serviceField.field_values != null)
                                    {
                                        foreach (var fieldValue in serviceField.field_values)
                                        {
                                            amountFieldList.Add(new PaymentServiceEnum(fieldValue.field_id,
                                                                                       fieldValue.name,
                                                                                       new MultiLanguageString(
                                                                                           fieldValue.name_en,
                                                                                           fieldValue.name_ru,
                                                                                           fieldValue.name_az),
                                                                                       fieldValue.value));
                                        }
                                    }

                                    serviceFieldList.Add(new PaymentServiceField(serviceField.id,
                                                                                 serviceField.service_id,
                                                                                 new MultiLanguageString(
                                                                                     serviceField.name_en,
                                                                                     serviceField.name_ru,
                                                                                     serviceField.name_az),
                                                                                 serviceField.name,
                                                                                 serviceField.service_name,
                                                                                 serviceField.type, serviceField.regexp,
                                                                                 serviceField.default_value,
                                                                                 serviceField.order_num,
                                                                                 amountFieldList,
                                                                                 serviceField.normalize_regexp,
                                                                                 serviceField.normalize_pattern));
                                }
                            }

                            var fixedAmountFieldList = new List<PaymentFixedAmount>();
                            if (service.amounts_list != null)
                            {
                                foreach (var amountField in service.amounts_list)
                                {
                                    fixedAmountFieldList.Add(new PaymentFixedAmount(amountField.service_id,
                                                                                    amountField.amount,
                                                                                    new MultiLanguageString(
                                                                                        amountField.name_en,
                                                                                        amountField.name_ru,
                                                                                        amountField.name_az)));
                                }
                            }

                            serviceList.Add(new Containers.MultiPayment.PaymentService(service.id, service.name,
                                                                                       new MultiLanguageString(
                                                                                           service.name_en,
                                                                                           service.name_ru,
                                                                                           service.name_az),
                                                                                       service.sub_name,
                                                                                       service.paypoint_payment_type,
                                                                                       service.type,
                                                                                       service.fixed_amount,
                                                                                       service.category_id,
                                                                                       service.min_amount,
                                                                                       service.max_amount,
                                                                                       service.assembly_id,
                                                                                       fixedAmountFieldList,
                                                                                       serviceFieldList));
                        }
                    }

                    resultList.Add(new Containers.MultiPayment.PaymentCategory(category.id, category.name,
                                                                               new MultiLanguageString(
                                                                                   category.name_en, category.name_ru,
                                                                                   category.name_az), serviceList));
                }

                //Log.Debug(EnumEx.GetStringFromArray(resultList));

                result.Categories = resultList;
                result.Code = ResultCodes.Ok;
                result.Sign = DoSign(request.TerminalId, result.SystemTime, terminalInfo.SignKey);
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return result;
        }
    }
}
