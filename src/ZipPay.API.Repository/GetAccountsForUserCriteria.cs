using System.Collections.Generic;
using System.Linq;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;

namespace ZipPay.API.Repository
{
    public class GetAccountsForUserCriteria : ICriteria<Account>
    {
        private readonly long _userId;

        public GetAccountsForUserCriteria(long userId)
        {
            _userId = userId;
        }

        public IList<Account> MatchQueryFrom(IQueryable<Account> dataEntities)
        {
            return dataEntities.Where(a => a.UserId == _userId).ToList();
        }
    }
}