using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    Id = row.ID,
                    Address = row.ADDRESS,
                    IdentityName = row.IDENTITY_NAME,
                    Ip = row.IsIPNull() ? String.Empty : row.IP,
                    LastCashcodeStatus = row.LAST_CASHCODE_STATUS,
                    LastStatusType = row.LAST_STATUS_TYPE,
                    LastStatusUpdate = row.LAST_STATUS_UPDATE,
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
                    DefaultCurrency = row.DEFAULT_CURRENCY > 0 ? true : false,
                    CurrencyRate = row.IsCURRENCY_RATENull() ? decimal.MinusOne : row.CURRENCY_RATE
                };

            return result;
        }
    }
}
