using System.Collections.Generic;
using ZipPay.API.Model;

namespace ZipPay.API.Interfaces
{
    public interface IAccountService
    {
        Account CreateAccount(Account isAny);
        IList<Account> GetUsersAccounts(long userId);
        IList<Account> GetAccounts();
    }
}
