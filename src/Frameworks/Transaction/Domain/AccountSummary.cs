namespace Transaction.Framework.Domain
{
    using Transaction.Framework.Types;

    public class AccountSummary
    {
        public int AccountNumber { get; set; }
        public Money Balance { get; set; }
        public bool IsWriteOff { get; set; }
        public decimal ConvertAmount { get; set; }
        public string ConversionCurrency { get; set; }
    }
}
