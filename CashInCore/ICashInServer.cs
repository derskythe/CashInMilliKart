using System.ServiceModel;
using Containers;
using Containers.Admin;
using Containers.MultiPayment;

namespace CashInCore
{
    [ServiceContract(Namespace = "http://cashin/CashInService",
        Name = "CashInServer"), XmlSerializerFormat]
    public interface ICashInServer
    {
        [OperationContract]
        string GetPublicKey();

        [OperationContract]
        TerminalInfoResult GetTerminalInfo(StandardRequest request);

        [OperationContract]
        AuthResult InitTerminal(int terminalId, string authKey, string publicKey);

        [OperationContract]
        PingResult Ping(PingRequest request);

        [OperationContract]
        StandardResult CommandReceived(StandardRequest request);

        [OperationContract]
        ProductResult ListProducts(StandardRequest request);

        [OperationContract]
        CurrenciesResult ListCurrencies(StandardRequest request);

        [OperationContract]
        StandardResult Payment(PaymentInfoByProducts request);

        [OperationContract]
        StandardResult Encashment(Encashment request);

        [OperationContract]
        StandardResult UpdateTerminalVersion(TerminalVersionRequest request);

        [OperationContract]
        StandardResult TerminalRestarted(StandardRequest request);

        [OperationContract]
        GetClientInfoResult GetClientInfo(GetClientInfoRequest request);

        [OperationContract]
        ListCheckTemplateResult ListCheckTemplate(StandardRequest request);

        [OperationContract]
        ListCheckTemplateResult ListCheckTemplateDigest(StandardRequest request);

        [OperationContract]
        BonusResponse GetBonusAmount(BonusRequest request);

        [OperationContract]
        CategoriesResult ListPaymentCategories(StandardRequest request);

        [OperationContract]
        PaymentServiceInfoResponse GetPaymentServiceInfo(PaymentServiceInfoRequest request);

        [OperationContract]
        StandardResult Pay(TerminalPaymentInfo request);

        [OperationContract]
        StandardResult UpdateTerminalVersionExt(TerminalVersionExtRequest request);

        [OperationContract]
        StandardResult CreditRequest(CreditRequest request);

        [OperationContract]
        GetClientInfoResult GetClientInfoExt(GetClientInfoRequest request);

        [OperationContract]
        CategoriesResult ListPaymentCategoriesExt(PaymentCategoriesRequest request);
    }
}
