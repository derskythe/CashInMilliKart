using System;
using System.Collections.Generic;
using Containers;
using Containers.Admin;

namespace Db
{
    class Convertor
    {
        public static Branch ToBranch(ds.V_BRANCHESRow row)
        {
            var result = new Branch
                {
                    Id = row.ID,
                    InsertDate = row.INSERT_DATE,
                    Name = row.NAME,
                    UpdateDate = row.UPDATE_DATE,
                    UserId = row.USER_ID,
                    Username = row.USERNAME
                };

            return result;
        }

        public static Branch ToBranch(ds.V_BRANCHES_TO_USERSRow row)
        {            
            var result = new Branch
            {
                Id = row.ID,
                InsertDate = row.INSERT_DATE,
                Name = row.IsNAMENull() ? String.Empty : row.NAME,
                UpdateDate = row.UPDATE_DATE,
                UserId = row.IsUSER_IDNull() ? 0 : Convert.ToInt32(row.USER_ID)
            };

            return result;
        }

        public static Product ToProduct(ds.V_PRODUCTSRow row)
        {
            var result = new Product
                {
                    Id = row.ID,
                    Name = row.NAME,
                    Assembly = row.ASSEMBLY,
                    NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                    NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                    NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                    CheckType = Convert.ToInt32(row.CHECK_TYPE)
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
                NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                CheckType = Convert.ToInt32(row.CHECK_TYPE)
            };

            return result;
        }

        public static Terminal ToTerminal(ds.V_LIST_TERMINALSRow row)
        {
            var terminalStatusDesc = new MultiLanguageString(
                row.IsTERMINAL_STATUS_ENNull() ? String.Empty : row.TERMINAL_STATUS_EN,
                row.IsTERMINAL_STATUS_RUNull() ? String.Empty : row.TERMINAL_STATUS_RU,
                row.IsTERMINAL_STATUS_AZNull() ? String.Empty : row.TERMINAL_STATUS_AZ
                );
            terminalStatusDesc.ReInit();

            var cashcodeDesc = new MultiLanguageString(
                row.IsCASHCODE_ENNull() ? String.Empty : row.CASHCODE_EN,
                row.IsCASHCODE_RUNull() ? String.Empty : row.CASHCODE_RU,
                row.IsCASHCODE_AZNull() ? String.Empty : row.CASHCODE_AZ
                );
            cashcodeDesc.ReInit();

            var printerStatusDesc = new MultiLanguageString(
                row.IsPRINTER_STATUS_ENNull() ? String.Empty : row.PRINTER_STATUS_EN,
                row.IsPRINTER_STATUS_RUNull() ? String.Empty : row.PRINTER_STATUS_RU,
                row.IsPRINTER_STATUS_AZNull() ? String.Empty : row.PRINTER_STATUS_AZ
                );
            printerStatusDesc.ReInit();

            var printerErrorStatusDesc = new MultiLanguageString(
                row.IsPRINTER_ERROR_STATUS_ENNull() ? String.Empty : row.PRINTER_ERROR_STATUS_EN,
                row.IsPRINTER_ERROR_STATUS_RUNull() ? String.Empty : row.PRINTER_ERROR_STATUS_RU,
                row.IsPRINTER_ERROR_STATUS_AZNull() ? String.Empty : row.PRINTER_ERROR_STATUS_AZ
                );
            printerErrorStatusDesc.ReInit();

            var printerExtErrorStatusDesc = new MultiLanguageString(
                row.IsPRINTER_EXT_ERROR_STATUS_ENNull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_EN,
                row.IsPRINTER_EXT_ERROR_STATUS_RUNull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_RU,
                row.IsPRINTER_EXT_ERROR_STATUS_ENNull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_EN
                );
            printerExtErrorStatusDesc.ReInit();

            var result = new Terminal
                {
                    Id = Convert.ToInt32(row.ID),
                    Address = row.ADDRESS,
                    IdentityName = String.Empty,
                    Ip = row.IsIPNull() ? String.Empty : row.IP,
                    LastCashcodeStatus = row.IsLAST_CASHCODE_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_STATUS),
                    LastStatusType = row.IsLAST_STATUS_TYPENull() ? -1 : Convert.ToInt32(row.LAST_STATUS_TYPE),
                    LastStatusUpdate = row.IsLAST_STATUS_UPDATENull() ? DateTime.MinValue : row.LAST_STATUS_UPDATE,
                    Name = row.NAME,
                    SignKey = row.IsSIGN_KEYNull() ? null : row.SIGN_KEY,
                    TmpKey = row.IsTMP_KEYNull() ? null : row.TMP_KEY,
                    BillsCount = row.IsBILLS_COUNTNull() ? 0 : row.BILLS_COUNT,
                    LastPrinterStatus = row.IsLAST_PRINTER_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_PRINTER_STATUS),
                    LastCashcodeError = row.IsLAST_CASHCODE_ERRORNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_ERROR),
                    LastCashcodeOutStatus = row.IsLAST_CASHCODE_OUT_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_OUT_STATUS),
                    LastCashcodeSuberror = row.IsLAST_CASHCODE_SUBERRORNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_SUBERROR),
                    LastPrinterErrorState = row.IsLAST_PRINTER_ERROR_STATENull() ? -1 : Convert.ToInt32(row.LAST_PRINTER_ERROR_STATE),
                    LastPrinterExtErrorState = row.IsLAST_PRINTER_EXT_ERROR_STATENull() ? -1 : Convert.ToInt32(row.LAST_PRINTER_EXT_ERROR_STATE),
                    TerminalStatusDesc = terminalStatusDesc,
                    CashcodeDesc = cashcodeDesc,
                    PrinterStatusDesc = printerStatusDesc,
                    PrinterErrorStatusDesc = printerErrorStatusDesc,
                    PrinterExtErrorStatusDesc = printerExtErrorStatusDesc,
                    CashcodeStatusName = row.IsCASHCODE_STATUS_NAMENull() ? String.Empty : row.CASHCODE_STATUS_NAME,
                    PrinterErrorStatusName = row.IsPRINTER_ERROR_STATUS_NAMENull() ? String.Empty : row.PRINTER_ERROR_STATUS_NAME,
                    PrinterExtErrorStatusName = row.IsPRINTER_ERROR_STATUS_NAMENull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_NAME,
                    PrinterStatusName = row.IsPRINTER_STATUS_NAMENull() ? String.Empty : row.PRINTER_STATUS_NAME,
                    TerminalStatusName = row.IsTERMINAL_STATUS_NAMENull() ? String.Empty : row.TERMINAL_STATUS_NAME,
                    TerminalStatusType = row.IsTERMINAL_STATUS_TYPENull() ? 0 : row.TERMINAL_STATUS_TYPE,
                    CashcodeStatusType = row.IsCASHCODE_STATUS_TYPENull() ? 0 : row.CASHCODE_STATUS_TYPE,
                    PrinterStatusType = row.IsPRINTER_STATUS_TYPENull() ? 0 : row.PRINTER_STATUS_TYPE,
                    Version = row.IsTERMINAL_VERSIONNull() ? String.Empty : row.TERMINAL_VERSION,
                    BranchId = row.IsBRANCH_IDNull() ? 0 : row.BRANCH_ID,
                    BranchName = row.IsBRANCH_NAMENull() ? String.Empty : row.BRANCH_NAME,
                    LastEncashment = row.IsLAST_ENCASHMENT_IDNull() ? 0 : Convert.ToInt32(row.LAST_ENCASHMENT_ID)
                };

            return result;
        }

        public static TerminalStatusCode ToTerminalStatusCode(ds.V_TERMINAL_STATUS_CODESRow row)
        {
            var terminalStatusDesc = new MultiLanguageString(
               row.IsTERMINAL_STATUS_ENNull() ? String.Empty : row.TERMINAL_STATUS_EN,
               row.IsTERMINAL_STATUS_RUNull() ? String.Empty : row.TERMINAL_STATUS_RU,
               row.IsTERMINAL_STATUS_AZNull() ? String.Empty : row.TERMINAL_STATUS_AZ
               );

            var result = new TerminalStatusCode
                {
                    Id = Convert.ToInt32(row.ID),
                    Name = row.IsNAMENull() ? String.Empty : row.NAME,
                    Type = Convert.ToInt32(row.TYPE),
                    Value = terminalStatusDesc
                };

            return result;
        }

        public static Terminal ToTerminal(ds.V_LIST_ENCASHMENTRow row)
        {
            var terminalStatusDesc = new MultiLanguageString(
                row.IsTERMINAL_STATUS_ENNull() ? String.Empty : row.TERMINAL_STATUS_EN,
                row.IsTERMINAL_STATUS_RUNull() ? String.Empty : row.TERMINAL_STATUS_RU,
                row.IsTERMINAL_STATUS_AZNull() ? String.Empty : row.TERMINAL_STATUS_AZ
                );
            terminalStatusDesc.ReInit();

            var cashcodeDesc = new MultiLanguageString(
                row.IsCASHCODE_ENNull() ? String.Empty : row.CASHCODE_EN,
                row.IsCASHCODE_RUNull() ? String.Empty : row.CASHCODE_RU,
                row.IsCASHCODE_AZNull() ? String.Empty : row.CASHCODE_AZ
                );
            cashcodeDesc.ReInit();

            var printerStatusDesc = new MultiLanguageString(
                row.IsPRINTER_STATUS_ENNull() ? String.Empty : row.PRINTER_STATUS_EN,
                row.IsPRINTER_STATUS_RUNull() ? String.Empty : row.PRINTER_STATUS_RU,
                row.IsPRINTER_STATUS_AZNull() ? String.Empty : row.PRINTER_STATUS_AZ
                );
            printerStatusDesc.ReInit();

            var printerErrorStatusDesc = new MultiLanguageString(
                row.IsPRINTER_ERROR_STATUS_ENNull() ? String.Empty : row.PRINTER_ERROR_STATUS_EN,
                row.IsPRINTER_ERROR_STATUS_RUNull() ? String.Empty : row.PRINTER_ERROR_STATUS_RU,
                row.IsPRINTER_ERROR_STATUS_AZNull() ? String.Empty : row.PRINTER_ERROR_STATUS_AZ
                );
            printerErrorStatusDesc.ReInit();

            var printerExtErrorStatusDesc = new MultiLanguageString(
                row.IsPRINTER_EXT_ERROR_STATUS_ENNull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_EN,
                row.IsPRINTER_EXT_ERROR_STATUS_RUNull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_RU,
                row.IsPRINTER_EXT_ERROR_STATUS_ENNull() ? String.Empty : row.PRINTER_EXT_ERROR_STATUS_EN
                );
            printerExtErrorStatusDesc.ReInit();

            var result = new Terminal
            {
                Id = Convert.ToInt32(row.TERMINAL_ID),
                Address = row.ADDRESS,
                IdentityName = row.IsIDENTITY_NAMENull() ? String.Empty : row.IDENTITY_NAME,
                Ip = row.IsIPNull() ? String.Empty : row.IP,
                LastCashcodeStatus = row.IsLAST_CASHCODE_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_STATUS),
                LastStatusType = row.IsLAST_STATUS_TYPENull() ? -1 : Convert.ToInt32(row.LAST_STATUS_TYPE),
                LastStatusUpdate = row.IsLAST_STATUS_UPDATENull() ? DateTime.MinValue : row.LAST_STATUS_UPDATE,
                Name = row.NAME,
                SignKey = null,
                TmpKey = null,
                BillsCount = row.IsBILLS_COUNTNull() ? 0 : row.BILLS_COUNT,
                LastPrinterStatus = row.IsLAST_PRINTER_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_PRINTER_STATUS),
                LastCashcodeError = row.IsLAST_CASHCODE_ERRORNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_ERROR),
                LastCashcodeOutStatus = row.IsLAST_CASHCODE_OUT_STATUSNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_OUT_STATUS),
                LastCashcodeSuberror = row.IsLAST_CASHCODE_SUBERRORNull() ? -1 : Convert.ToInt32(row.LAST_CASHCODE_SUBERROR),
                LastPrinterErrorState = row.IsLAST_PRINTER_ERROR_STATENull() ? -1 : Convert.ToInt32(row.LAST_PRINTER_ERROR_STATE),
                LastPrinterExtErrorState = row.IsLAST_PRINTER_EXT_ERROR_STATENull() ? -1 : Convert.ToInt32(row.LAST_PRINTER_EXT_ERROR_STATE),
                TerminalStatusDesc = terminalStatusDesc,
                CashcodeDesc = cashcodeDesc,
                PrinterStatusDesc = printerStatusDesc,
                PrinterErrorStatusDesc = printerErrorStatusDesc,
                PrinterExtErrorStatusDesc = printerExtErrorStatusDesc,
                BranchId = row.IsBRANCH_IDNull() ? 0 : row.BRANCH_ID,
                BranchName = row.IsBRANCH_NAMENull() ? String.Empty : row.BRANCH_NAME,
                LastEncashment = row.IsLAST_ENCASHMENT_IDNull() ? 0 : Convert.ToInt32(row.LAST_ENCASHMENT_ID)
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

        public static User ToUser(ds.V_LIST_USERSRow row, List<AccessRole> fields, List<Branch> branches)
        {
            var result = new User
            {
                Id = Convert.ToInt32(row.ID),
                Username = row.USERNAME,
                Password = row.PASSWORD,
                InsertDate = row.INSERT_DATE,
                LastUpdate = row.UPDATE_DATE,
                Active = row.ACTIVE > 0,
                RoleFields = fields,
                Branches = branches,
                Salt = row.SALT
            };

            return result;
        }

        public static UserSession ToUserSession(ds.V_LIST_ACTIVE_SESSIONSRow row, List<AccessRole> fields, List<Branch> branches)
        {
            var user = new User
                {
                    Active = true,
                    InsertDate = row.IsINSERT_DATENull() ? DateTime.MinValue : row.INSERT_DATE,
                    LastUpdate = row.IsUPDATE_DATENull() ? DateTime.MinValue : row.UPDATE_DATE,
                    Password = String.Empty,
                    Salt = String.Empty,
                    Username = row.USERNAME,
                    Id = Convert.ToInt32(row.USER_ID),
                    RoleFields = fields,
                    Branches = branches
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

        public static Encashment ToEncashment(ds.V_LIST_ENCASHMENTRow row, List<EncashmentCurrency> currencies, Terminal terminal, String username)
        {
            var result = new Encashment
            {
                Currencies = currencies.ToArray(),
                Id = Convert.ToInt32(row.ID),
                InsertDate = row.INSERT_DATE,
                TerminalId = row.IsTERMINAL_IDNull() ? Int32.MinValue : Convert.ToInt32(row.TERMINAL_ID),
                Terminal = terminal,
                Username = username,
                Count = 0
            };

            return result;
        }

        public static Encashment ToEncashment(ds.V_LIST_ENCASHMENTRow row, List<EncashmentCurrency> currencies, Terminal terminal, String username, List<Banknote> banknotes)
        {
            var result = new Encashment
                {
                    Currencies = currencies.ToArray(),
                    Id = Convert.ToInt32(row.ID),
                    InsertDate = row.INSERT_DATE,
                    TerminalId = row.IsTERMINAL_IDNull() ? Int32.MinValue : Convert.ToInt32(row.TERMINAL_ID),
                    Terminal = terminal,
                    Username = username,
                    Banknotes = banknotes,
                    Count = banknotes.Count
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

        public static ProductHistory ToProductHistory(ds.V_PRODUCTS_HISTORYRow row, List<ProductHistoryValue> values, List<BanknoteSummary> banknotes)
        {
            var desc = new MultiLanguageString(
                row.IsPAYMENT_TYPE_ENNull() ? String.Empty : row.PAYMENT_TYPE_EN,
                row.IsPAYMENT_TYPE_RUNull() ? String.Empty : row.PAYMENT_TYPE_RU,
                row.IsPAYMENT_TYPE_AZNull() ? String.Empty : row.PAYMENT_TYPE_AZ
                );
            desc.ReInit();            

            var result = new ProductHistory
                {
                    Address = row.IsADDRESSNull() ? String.Empty : row.ADDRESS,
                    Amount = Convert.ToSingle(row.AMOUNT),
                    CurrencyId = row.CURRENCY_ID,
                    Id = Convert.ToInt64(row.ID),
                    IdentityName = row.IsIDENTITY_NAMENull() ? String.Empty : row.IDENTITY_NAME,
                    InsertDate = row.INSERT_DATE,
                    Name = row.IsNAMENull() ? String.Empty : row.NAME,
                    NameAz = row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ,
                    NameRu = row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                    NameEn = row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                    Values = values,
                    TerminalId = Convert.ToInt64(row.TERMINAL_ID),
                    TerminalDate = row.IsTERMINAL_DATENull() ? DateTime.MinValue : row.TERMINAL_DATE,
                    ProductId = Convert.ToInt64(row.PRODUCT_ID),
                    EncashmentId = row.IsENCASHMENT_IDNull() ? 0 : Convert.ToInt64(row.ENCASHMENT_ID),
                    ProductName = row.IsPRODUCT_NAMENull() ? String.Empty : row.PRODUCT_NAME,
                    Rate = row.IsRATENull() ? 1f : Convert.ToSingle(row.RATE),
                    TransactionId = row.IsTRANSACTION_IDNull() ? String.Empty : row.TRANSACTION_ID,
                    Banknotes = banknotes,
                    CreditNumber = row.IsCREDIT_NUMBERNull() ? String.Empty : row.CREDIT_NUMBER,
                    PaymentType = row.IsPAYMENT_TYPENull() ? 0 : Convert.ToInt32(row.PAYMENT_TYPE),
                    PaymentTypeName = desc
                };

            return result;
        }

        public static Banknote ToBanknote(ds.V_BANKNOTESRow row)
        {
            var result = new Banknote(
                row.ID,
                row.AMOUNT,
                row.TERMINAL_ID,
                row.INSERT_DATE,
                row.CURRENCY_ID,
                row.IsENCASHMENT_IDNull() ? 0 : row.ENCASHMENT_ID,
                row.HISTORY_ID
                );

            return result;
        }

        public static CheckType ToCheckType(ds.V_CHECK_TYPESRow row)
        {
            var value = new MultiLanguageString(
                row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ
                );
            value.ReInit();

            var result = new CheckType(
                row.ID,
                value
                );

            return result;
        }

        public static CheckFieldType ToCheckFieldType(ds.V_CHECK_FIELD_TYPESRow row)
        {
            var value = new MultiLanguageString(
                row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ
                );
            value.ReInit();

            var result = new CheckFieldType(
                row.ID,
                value
                );

            return result;
        }

        public static CheckField ToCheckField(ds.V_CHECK_FIELDSRow row)
        {
            var result = new CheckField(
                row.ID,
                row.CHECK_ID,
                row.IsIMAGENull() ? null : row.IMAGE,
                row.IsVALUENull() ? String.Empty : row.VALUE,
                row.FIELD_TYPE,
                row.ORDER_NUMBER
                );

            return result;
        }

        public static CheckTemplate ToCheckTemplate(ds.V_CHECKSRow row, CheckType type, List<CheckField> fields)
        {
            var result = new CheckTemplate(
                row.ID,
                type,
                row.LANGUAGE,
                row.ACTIVE > 0,
                row.IsINSERT_DATENull() ? DateTime.MinValue : row.INSERT_DATE,
                row.IsUPDATE_DATENull() ? DateTime.MinValue : row.UPDATE_DATE,
                fields
            );

            return result;
        }

        public static CheckTemplate ToCheckTemplate(ds.V_CHECKSRow row, List<CheckField> fields)
        {
            var value = new MultiLanguageString(
                row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ
                );

            var type = new CheckType(
                row.CHECK_TYPE,
                value
                );

            var result = new CheckTemplate(
                row.ID,
                type,
                row.LANGUAGE,
                row.ACTIVE > 0,
                row.IsINSERT_DATENull() ? DateTime.MinValue : row.INSERT_DATE,
                row.IsUPDATE_DATENull() ? DateTime.MinValue : row.UPDATE_DATE,
                fields
            );

            return result;
        }

        public static CheckTemplate ToCheckTemplate(ds.V_CHECKSRow row)
        {
            var value = new MultiLanguageString(
                row.IsNAME_ENNull() ? String.Empty : row.NAME_EN,
                row.IsNAME_RUNull() ? String.Empty : row.NAME_RU,
                row.IsNAME_AZNull() ? String.Empty : row.NAME_AZ
                );

            var type = new CheckType(
                row.CHECK_TYPE,
                value
                );

            var result = new CheckTemplate(
                row.ID,
                type,
                row.LANGUAGE,
                row.ACTIVE > 0,
                row.IsINSERT_DATENull() ? DateTime.MinValue : row.INSERT_DATE,
                row.IsUPDATE_DATENull() ? DateTime.MinValue : row.UPDATE_DATE                
            );

            return result;
        }

        public static ClientInfo ToClientInfo(ds.V_CASHIN_GET_ACCOUNT_INFORow row)
        {
            var result = new ClientInfo(
                row.CRD_NUMBER,
                row.IsFULL_NAMENull() ? String.Empty : row.FULL_NAME,
                                        row.IsPASSPORTNUMBERNull() ? String.Empty : row.PASSPORTNUMBER,
                                        row.IsCREDIT_ACCOUNTNull() ? String.Empty : row.CREDIT_ACCOUNT,
                                        row.IsCLIENT_ACCOUNTNull() ? String.Empty : row.CLIENT_ACCOUNT,
                                        row.IsAMOUNT_LEFTNull() ? 0f : row.AMOUNT_LEFT,
                                        row.IsAMOUNT_LATENull() ? 0f : row.AMOUNT_LATE,
                                        row.IsCURRENCYNull() ? String.Empty : row.CURRENCY,
                                        row.IsBEGIN_DATENull() ? DateTime.MinValue : row.BEGIN_DATE,
                                        row.IsCURRENCY_RATENull() ? 0f : row.CURRENCY_RATE,
                                        row.IsCLNCODENull() ? String.Empty : row.CLNCODE,
                                        row.IsCREDIT_AMOUNTNull() ? 0f : row.CREDIT_AMOUNT,
                                        row.IsCREDIT_NAMENull() ? String.Empty : row.CREDIT_NAME);
            //row.FULL_NAME = FirstUpper(row.FULL_NAME);
            return result;
        }

        public static BanknoteSummary ToBanknoteSummary(ds.V_BANKNOTES_SUMMARY_BY_HISTORYRow row)
        {
            var result = new BanknoteSummary
                {
                    Amount = Convert.ToSingle(row.AMOUNT),
                    CountAll = row.IsCOUNT_ALLNull() ? 0 : Convert.ToInt32(row.COUNT_ALL),
                    EncashmentId = row.IsENCASHMENT_IDNull() ? 0 : Convert.ToInt32(row.ENCASHMENT_ID),
                    HistoryId = Convert.ToInt32(row.HISTORY_ID),
                    TerminalId = Convert.ToInt32(row.TERMINAL_ID),
                    Currency = row.CURRENCY_ID
                };

            return result;
        }

        public static BanknoteSummary ToBanknoteSummary(ds.V_BANKNOTES_SUMMARY_BY_TERMRow row)
        {
            var result = new BanknoteSummary
            {
                Amount = Convert.ToSingle(row.AMOUNT),
                CountAll = row.IsCOUNT_ALLNull() ? 0 : Convert.ToInt32(row.COUNT_ALL),
                EncashmentId = row.IsENCASHMENT_IDNull() ? 0 : Convert.ToInt32(row.ENCASHMENT_ID),
                HistoryId = Convert.ToInt32(row.HISTORY_ID),
                TerminalId = Convert.ToInt32(row.TERMINAL_ID),
                Currency = row.CURRENCY_ID
            };

            return result;
        }

        public static BanknoteSummary ToBanknoteSummary(ds.V_BANKNOTES_SUMMARY_ENCASHMENTRow row)
        {
            var result = new BanknoteSummary
            {
                Amount = Convert.ToSingle(row.AMOUNT),
                CountAll = row.IsCOUNT_ALLNull() ? 0 : Convert.ToInt32(row.COUNT_ALL),
                EncashmentId = row.IsENCASHMENT_IDNull() ? 0 : Convert.ToInt32(row.ENCASHMENT_ID),
                HistoryId = Convert.ToInt32(row.HISTORY_ID),
                TerminalId = Convert.ToInt32(row.TERMINAL_ID),
                Currency = row.CURRENCY_ID
            };

            return result;
        }

        //private static String FirstUpper(String value)
        //{
        //    if (String.IsNullOrEmpty(value))
        //    {
        //        return String.Empty;
        //    }

        //    var values = value.Split(' ');
        //    var buffer = new StringBuilder();
        //    foreach (var s in values)
        //    {
        //        buffer.Append(s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant()).Append(" ");
        //    }

        //    return buffer.ToString().Trim();
        //}
    }
}
