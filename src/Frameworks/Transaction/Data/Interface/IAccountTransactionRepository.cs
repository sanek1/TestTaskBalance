namespace Transaction.Framework.Data.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Transaction.Framework.Data.Entities;

    public interface IAccountTransactionRepository
    {
        Task Update(AccountTransactionEntity accountSummaryEntity);
        Task<List<AccountTransactionEntity>> GetTransactions(int accountNumber);
        Task Create(AccountTransactionEntity accountTransactionEntity, AccountSummaryEntity accountSummaryEntity);
    }
}
