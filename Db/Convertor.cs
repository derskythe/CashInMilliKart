using System;
using System.Collections.Generic;
using Containers;

namespace Db
{
    class Convertor
    {
        public static Product ToProduct(ds.V_PRODUCTSRow row)
        {
            var result = new Product
                {
                    Id = row.ID,
                    Name = row.NAME,
                    Assembly = row.ASSEMBLY,
                    NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                    NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                    NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU
                };

            return result;
        }

        public static Product ToProduct(ds.V_PRODUCTS_TO_TERMINALSRow row)
        {
            var result = new Product
            {
                Id = row.ID,
                Name = row.NAME,
                Assembly = row.ASSEMBLY,
                NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU
            };

            return result;
        }

        public static Terminal ToTerminal(ds.V_LIST_TERMINALSRow row)
        {
            var result = new Terminal
                {
                    Id = Convert.ToInt32(row.ID),
                    Address = row.ADDRESS,
                    IdentityName = row.IDENTITY_NAME,
                    Ip = row.IsIPNull() ? String.Empty : row.IP,
                    LastCashcodeStatus = row.IsLAST_CASHCODE_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_STATUS),
                    LastStatusType = row.IsLAST_STATUS_TYPENull() ? -1 : Convert.ToInt32(row.LAST_STATUS_TYPE),
                    LastStatusUpdate = row.IsLAST_STATUS_UPDATENull() ? DateTime.MinValue : row.LAST_STATUS_UPDATE,
                    Name = row.NAME,
                    SignKey = row.IsSIGN_KEYNull() ? null : row.SIGN_KEY,
                    TmpKey = row.IsTMP_KEYNull() ? null : row.TMP_KEY

                };

            return result;
        }

        public static Currency ToCurrency(ds.V_LIST_CURRENCIESRow row)
        {
            var result = new Currency
                {
                    Id = row.ID,
                    IsoName = row.ISO_NAME,
                    Name = row.NAME,
                    DefaultCurrency = row.DEFAULT_CURRENCY > 0,
                    CurrencyRate = row.IsRATENull() ? 1 : row.RATE
                };

            return result;
        }

        public static RoleField ToRoleField(ds.V_ROLES_TO_USERSRow row)
        {
            var result = new RoleField
                {
                    Section = row.IsSECTIONNull() ? String.Empty : row.SECTION
                };

            return result;
        }

        public static AccessRole ToAccessRole(ds.V_ROLES_TO_USERSRow row)
        {
            var result = new AccessRole
                {
                    Id = Convert.ToInt32(row.ROLE_ID),
                    Name = row.NAME,
                    NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                    NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                    NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                    Section = row.SECTION
                };

            return result;
        }

        public static AccessRole ToAccessRole(ds.V_ROLESRow row)
        {
            var result = new AccessRole
            {
                Id = Convert.ToInt32(row.ID),
                Name = row.NAME,
                NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                Section = String.Empty
            };

            return result;
        }

        public static User ToUser(ds.V_LIST_USERSRow row)
        {
            var result = new User
                {
                    Id = Convert.ToInt32(row.ID),
                    Username = row.USERNAME,
                    Password = row.PASSWORD,
                    InsertDate = row.INSERT_DATE,
                    LastUpdate = row.UPDATE_DATE,
                    Active = row.ACTIVE > 0,
                    Salt = row.SALT
                };

            return result;
        }

        public static User ToUser(ds.V_LIST_USERSRow row, List<AccessRole> fields)
        {
            var result = new User
            {
                Id = Convert.ToInt32(row.ID),
                Username = row.USERNAME,
                Password = row.PASSWORD,
                InsertDate = row.INSERT_DATE,
                LastUpdate = row.UPDATE_DATE,
                Active = row.ACTIVE > 0,
                RoleFields = fields.ToArray(),
                Salt = row.SALT
            };

            return result;
        }

        public static UserSession ToUserSession(ds.V_LIST_ACTIVE_SESSIONSRow row, List<AccessRole> fields)
        {
            var user = new User
                {
                    Active = true,
                    InsertDate = row.IsINSERT_DATENull() ? DateTime.MinValue : row.INSERT_DATE,
                    LastUpdate = row.IsUPDATE_DATENull() ? DateTime.MinValue : row.UPDATE_DATE,
                    Password = String.Empty,
                    Salt = String.Empty,
                    Username = row.IsUSERNAMENull() ? String.Empty : row.USERNAME,
                    Id = Convert.ToInt32(row.USER_ID),
                    RoleFields = fields.ToArray()
                };

            var result = new UserSession
                {
                    Sid = row.SID,
                    LastUpdate = row.LAST_UPDATE,
                    StartDate = row.START_DATE,
                    User = user
                };

            return result;
        }

        public static EncashmentCurrency ToEncashmentCurrency(ds.V_LIST_ENCASHMENT_CURRENCIESRow row)
        {
            var result = new EncashmentCurrency
                {
                    Amount = Convert.ToInt32(row.AMOUNT),
                    Currency = row.CURRENCY_ID
                };

            return result;
        }

        public static Encashment ToEncashment(ds.V_LIST_ENCASHMENTRow row, List<EncashmentCurrency> currencies)
        {
            var result = new Encashment
                {
                    Currencies = currencies.ToArray(),
                    Id = Convert.ToInt32(row.ID),
                    InsertDate = row.INSERT_DATE,
                    TerminalId = row.IsTERMINAL_IDNull() ? Int32.MinValue : Convert.ToInt32(row.TERMINAL_ID),
                    UserId = row.IsUSER_IDNull() ? Int32.MinValue : Convert.ToInt32(row.USER_ID)
                };

            return result;
        }

        public static ProductHistoryValue ToProductHistoryValue(ds.V_PRODUCTS_HISTORY_VALUESRow row)
        {
            var result = new ProductHistoryValue
                {
                    Id = row.ID,
                    Value = row.VALUE
                };

            return result;
        }

        public static ProductHistory ToProductHistory(ds.V_PRODUCTS_HISTORYRow row, List<ProductHistoryValue> values)
        {
            var result = new ProductHistory
                {
                    Address = row.IsADDRESSNull() ? String.Empty : row.ADDRESS,
                    Amount = row.AMOUNT,
                    CurrencyId = row.CURRENCY_ID,
                    Id = row.ID,
                    IdentityName = row.IsIDENTITY_NAMENull() ? String.Empty : row.IDENTITY_NAME,
                    InsertDate = row.INSERT_DATE,
                    Name = row.IsNAMENull() ? String.Empty : row.NAME,
                    NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                    NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                    NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                    Values = values,
                    TerminalId = row.TERMINAL_ID,
                    TerminalDate = row.IsTERMINAL_DATENull() ? DateTime.MinValue : row.TERMINAL_DATE,
                    ProductId = row.PRODUCT_ID,
                    ProductName = row.IsPRODUCT_NAMENull() ? String.Empty : row.PRODUCT_NAME,
                    Rate = row.IsRATENull() ? 1 : row.RATE,
                    TransactionId = row.IsTRANSACTION_IDNull() ? String.Empty : row.TRANSACTION_ID
                };

            return result;
        }
    }
}
