using System.Collections.Generic;
using System.Linq;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;

namespace ZipPay.API.Repository
{
    public class FindUserByIdCriteria : ICriteria<User>
    {
        private readonly long _userId;

        public FindUserByIdCriteria(long userId)
        {
            _userId = userId;
        }

        public IList<User> MatchQueryFrom(IQueryable<User> dataEntities)
        {
            var result = dataEntities.Where(u => u.Id == _userId);
            return result.ToList();
        }
    }
}
