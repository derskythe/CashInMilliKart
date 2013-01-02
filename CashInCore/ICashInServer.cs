using System.ServiceModel;
using Containers;

namespace CashInCore
{
    [ServiceContract(Namespace = "http://cashin/CashInService",
        Name = "CashInServer"), XmlSerializerFormat]
    public interface ICashInServer
    {
        [OperationContract]
        string GetPublicKey();
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
    }
}
