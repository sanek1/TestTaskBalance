﻿namespace Transaction.Framework.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Transaction.Framework.Data.Entities;
    using Transaction.Framework.Data.Interface;
    using Transaction.Framework.Exceptions;
    using Transaction.Framework.Types;

    public class AccountTransactionRepository : IAccountTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<AccountSummaryEntity> _accountSummaryEntity;
        private readonly DbSet<AccountTransactionEntity> _accountTransactionEntity;        

        public AccountTransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _accountSummaryEntity = _dbContext.Set<AccountSummaryEntity>();
            _accountTransactionEntity = _dbContext.Set<AccountTransactionEntity>();
        }

        public async Task Update(AccountTransactionEntity accountSummaryEntity)
        {
            
            await _accountTransactionEntity.AddAsync(accountSummaryEntity);
            await _dbContext.SaveChangesAsync();
            bool isSaved = false; ;
            //while (!isSaved)
            //{
            //    try
            //    {
            //        await _dbContext.SaveChangesAsync();
            //        isSaved = true;
            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        foreach (var entry in ex.Entries)
            //        {
            //            if (entry.Entity is AccountSummaryEntity)
            //            {
            //                var databaseValues = entry.GetDatabaseValues();

            //                if (databaseValues != null)
            //                {
            //                    entry.OriginalValues.SetValues(databaseValues);
            //                    //CalculateNewBalance();

            //                    //void CalculateNewBalance()
            //                    //{
            //                    //    var balance = (decimal)entry.OriginalValues["Balance"];
            //                    //    var amount = accountTransactionEntity.Amount;

            //                    //    if (accountTransactionEntity.TransactionType == TransactionType.Deposit.ToString())
            //                    //    {
            //                    //        accountSummaryEntity.Balance =
            //                    //        balance += amount;
            //                    //    }
            //                    //    else if (accountTransactionEntity.TransactionType == TransactionType.Withdrawal.ToString())
            //                    //    {
            //                    //        if (amount > balance)
            //                    //            throw new InsufficientBalanceException();

            //                    //        accountSummaryEntity.Balance =
            //                    //        balance -= amount;
            //                    //    }
            //                    //}
            //                }
            //                else
            //                {
            //                    throw new NotSupportedException();
            //                }
            //            }
            //        }
            //    }
            //}

        }

        public async Task<List<AccountTransactionEntity>> GetTransactions(int accountNumber)
        {
            return await _accountTransactionEntity.Where(e => e.AccountNumber == accountNumber).ToListAsync();
        }

        public async Task Create(AccountTransactionEntity accountTransactionEntity, AccountSummaryEntity accountSummaryEntity)
        {
            _accountTransactionEntity.Add(accountTransactionEntity);

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
