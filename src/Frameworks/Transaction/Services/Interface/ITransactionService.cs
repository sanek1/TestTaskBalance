namespace Transaction.Framework.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Transaction.Framework.Domain;

    public interface ITransactionService
    {
        Task<AccountSummary> GetBalance(int accountNumber, string currency);
        Task<List<TransactionResult>> GetTransactions(int accountNumber);
        Task<AccountSummary> SetBalance(AccountSummary AccountSummary);
    }
}
