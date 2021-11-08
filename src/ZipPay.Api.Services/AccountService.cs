using System;
using System.Collections.Generic;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;
using ZipPay.API.Repository;

namespace ZipPay.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;

        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account CreateAccount(Account newAccount)
        {
            var result = _accountRepository.Add(newAccount);
            newAccount.Id = result;

            return newAccount;
        }

        public IList<Account> GetUsersAccounts(long userId)
        {
            var result = _accountRepository.Match(new GetAccountsForUserCriteria(userId));
            return result;
        }

        public IList<Account> GetAccounts()
        {
            var result = _accountRepository.Get();
            return result;
        }
    }
}
