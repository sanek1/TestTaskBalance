namespace Transaction.Framework.Validation
{
    using System.Threading.Tasks;
    using Transaction.Framework.Domain;
    using Transaction.Framework.Exceptions;
    using Transaction.Framework.Types;

    public static class TransactionValidation
    {

        public static async Task Validate(this AccountSummary accountSummary, int accountNumber)
        {

            if (accountSummary.Balance.Amount <= 0)
            {
                throw new InvalidAmountException(accountSummary.Balance.Amount);
            }
            if (accountSummary == null)
            {
                throw new InvalidAccountNumberException(accountNumber);
            }
            
            await Task.CompletedTask;
        }
    }
}
