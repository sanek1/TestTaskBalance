namespace Transaction.Framework.Types
{
    using Transaction.Framework.Exceptions;

    public struct Money 
    {
        public Money(decimal amount, Currency currency, string nameCurrency)
        {
            Amount = amount;
            Currency = currency;
            NameCurrency = nameCurrency;
        }

        public decimal Amount { get; private set; }
        public Currency Currency { get;  }

        public string NameCurrency { get; private set; }

    }
}
