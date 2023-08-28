namespace Transaction.Framework.Services
{
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using Transaction.Framework.Data.Entities;
    using Transaction.Framework.Data.Interface;
    using Transaction.Framework.Domain;
    using Transaction.Framework.Services.Interface;
    using Transaction.Framework.Types;
    using Transaction.Framework.Extensions;
    using Transaction.Framework.Validation;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Transaction.Framework.Exceptions;

    public class TransactionService : ITransactionService
    {
        private readonly IAccountSummaryRepository _accountSummaryRepository;
        private readonly IAccountTransactionRepository _accountTransactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly decimal _currencyGBR = 100;

        public TransactionService(IAccountSummaryRepository accountSummaryRepository, IAccountTransactionRepository accountTransactionRepository, IMapper mapper, ILogger<TransactionService> logger)
        {
            _accountSummaryRepository = accountSummaryRepository ?? throw new ArgumentNullException(nameof(accountSummaryRepository));
            _accountTransactionRepository = accountTransactionRepository ?? throw new ArgumentNullException(nameof(accountTransactionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AccountSummary> GetBalance(int accountNumber, string currency)
        {
            var accountSummary = await GetAccountSummary(accountNumber);
            await accountSummary.Validate(accountNumber);
            return await ConvertCurrency(accountSummary, currency);
        }


        public async Task<AccountSummary> SetBalance(AccountSummary AccountSummary)
        {
            await AccountSummary.Validate(AccountSummary.AccountNumber);
            var result = await UpdateSummary(AccountSummary);
            return result;
        }

        public async Task<List<TransactionResult>> GetTransactions(int accountNumber)
        {
            var transactionResult = await GetTransactionsSummary(accountNumber);
            return transactionResult;
        }

        private async Task<AccountSummary> UpdateSummary(AccountSummary accountSummary)
        {

            if (await WriteOffCheck(accountSummary) && accountSummary.IsWriteOff)
            {
                throw new InsufficientBalanceException();
            }

            var accountSummaryEntity = _mapper.Map<AccountSummaryEntity>(accountSummary);

            await _accountSummaryRepository.Update(accountSummaryEntity);
            var currentSummary = await GetAccountSummary(accountSummary.AccountNumber);

            await _accountTransactionRepository.Create(new AccountTransactionEntity() { 
            AccountNumber = accountSummary.AccountNumber,
            TransactionType = TransactionType.SetBalance.ToString(),
            Amount = accountSummary.Balance.Amount,
            Date = DateTime.UtcNow,
            Description = "The balance has been changed from "
            + currentSummary.Balance.Amount.ToString() 
            + " to "+ accountSummaryEntity.Balance.ToString() + " for user "+ currentSummary.AccountNumber
            }, accountSummaryEntity);

            return currentSummary;
        }

        private async Task<bool>  WriteOffCheck(AccountSummary accountSummary) {
            var accountSummaryDB = await GetAccountSummary(accountSummary.AccountNumber);
            return await Task.FromResult((accountSummaryDB.Balance.Amount - accountSummary.Balance.Amount) < 0 ? true : false);  

        }


        #region private helpers

        private async Task<AccountSummary> GetAccountSummary(int accountNumber)
        {
            var accountSummaryEntity = await _accountSummaryRepository
                .Read(accountNumber);

            return _mapper.Map<AccountSummary>(accountSummaryEntity);
        }

        private async Task<AccountSummary> ConvertCurrency(AccountSummary accountSummary, string currency)
        {
            switch (currency)
            {
                case "RUB":
                    break;
                case "USD":
                    accountSummary.ConvertAmount = accountSummary.Balance.Amount / _currencyGBR;
                    accountSummary.ConversionCurrency = currency;
                    break;
                default:
                    break;
            }

            return accountSummary;
        }

        private async Task<List<TransactionResult>> GetTransactionsSummary(int accountNumber)
        {
            var accountSummaryEntity = await _accountTransactionRepository
                .GetTransactions(accountNumber);
            return _mapper.Map<List<TransactionResult>>(accountSummaryEntity);
        }

        #endregion
    }
}
