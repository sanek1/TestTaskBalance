namespace Transaction.Framework.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Transaction.Framework.Data.Entities;
    using Transaction.Framework.Data.Interface;
    using Transaction.Framework.Domain;
    using Transaction.Framework.Types;

    public class AccountSummaryRepository : IAccountSummaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<AccountSummaryEntity> _accountSummaryEntity;

        public AccountSummaryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _accountSummaryEntity = _dbContext.Set<AccountSummaryEntity>();
        }

        public async Task<AccountSummaryEntity> Read(int accountNumber)
        {
            return await _accountSummaryEntity.AsNoTracking()
                .FirstOrDefaultAsync(e => e.AccountNumber == accountNumber);
        }

        public async Task Update(AccountSummaryEntity accountSummaryEntity)
        {
            var dbAccount = await Read(accountSummaryEntity.AccountNumber);
            dbAccount.Balance = accountSummaryEntity.Balance;
            dbAccount.Currency = accountSummaryEntity.Currency;

            _accountSummaryEntity.Update(dbAccount);

            bool isSaved = false;

            while (!isSaved)
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    isSaved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is AccountSummaryEntity)
                        {
                            var databaseValues = entry.GetDatabaseValues();

                            if (databaseValues != null)
                            {
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                    }
                }
            }

        }

    }
}
