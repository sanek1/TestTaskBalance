namespace Transaction.Framework.Data.Interface
{
    using System.Threading.Tasks;
    using Transaction.Framework.Data.Entities;
    using Transaction.Framework.Domain;

    public interface IAccountSummaryRepository
    {
        Task<AccountSummaryEntity> Read(int accountNumber);
        Task Update(AccountSummaryEntity accountSummaryEntity);
    }
}
