using System.Collections.Generic;
using System.Linq;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;

namespace ZipPay.API.Repository
{
    public class FindEmailCriteria : ICriteria<User>
    {
        private readonly string _emailAddress;

        public FindEmailCriteria(string emailAddress)
        {
            _emailAddress = emailAddress;
        }

        public IList<User> MatchQueryFrom(IQueryable<User> dataEntities)
        {
            var result = dataEntities.Where(u => u.EmailAddress == _emailAddress);
            return result.ToList();
        }
    }
}
