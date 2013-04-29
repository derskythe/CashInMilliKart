namespace Containers.Enums
{
    public enum PaymentOperationType
    {
        Unknown = 0,
        CreditPaymentByClientCode = 11,
        CreditPaymentByPassportAndAccount = 12,
        CreditPaymentBolcard = 13,
        DebitPaymentByClientCode = 21,
        DebitPaymentByPassportAndAccount = 22,
        GoldenPay = 33,
        Komtek = 34,
        AvirTel = 32,
        Aztelekom = 31,
        Bes = 41
    }
}
