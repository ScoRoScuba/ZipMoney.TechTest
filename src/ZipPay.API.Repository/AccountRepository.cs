using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;

namespace ZipPay.API.Repository
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly IMySqlServerDatabaseContext _databaseContext;

        public AccountRepository(IMySqlServerDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public long Add(Account item)
        {
            _databaseContext.Accounts.Add(item);
            _databaseContext.SaveChanges();

            return item.Id;
        }

        public Account Get(long id)
        {
            var acct = _databaseContext.Accounts.FirstOrDefault(u => u.Id == id);
            return acct;
        }

        public List<Account> Get()
        {
            return _databaseContext.Accounts.ToList();
        }

        public IList<Account> Match(ICriteria<Account> criteria)
        {
            return criteria.MatchQueryFrom(_databaseContext.Accounts);
        }
    }
}
