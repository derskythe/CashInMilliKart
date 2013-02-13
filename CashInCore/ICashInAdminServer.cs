using System;
using System.ServiceModel;
using Containers;
using Containers.Admin;

namespace CashInCore
{
    [ServiceContract(Namespace = "http://cashin/CashInAdminService",
        Name = "CashInAdminService"), XmlSerializerFormat]
    public interface ICashInAdminServer
    {
        [OperationContract]
        LoginResult Login(String username, String password);

        [OperationContract]
        SessionResult CheckSession(String sid);

        [OperationContract]
        StandardResult SaveUser(String sid, User userInfo);

        [OperationContract]
        StandardResult DeleteUser(String sid, int id);

        [OperationContract]
        StandardResult SetStatusCode(String sid, int terminalId, int statusCode);

        [OperationContract]
        StandardResult SaveUserRole(String sid, int userId, int roleId);

        [OperationContract]
        StandardResult SaveCurrency(String sid, Currency currency);

        [OperationContract]
        StandardResult SaveCurrencyRate(String sid, String currency, decimal rate);

        [OperationContract]
        ListUsersResult ListUsers(String sid);

        [OperationContract]
        ListRolesResult ListRoles(String sid);

        [OperationContract]
        ListTerminalsResult ListTerminals(String sid);

        [OperationContract]
        ListEncashmentResult ListEncashment(String sid);

        [OperationContract]
        ListProductsResult ListProducts(String sid);

        [OperationContract]
        ListProductHistory ListProductHistory(String sid);
    }
}