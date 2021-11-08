using System.Collections.Generic;
using System.Linq;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;

namespace ZipPay.API.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly IMySqlServerDatabaseContext _databaseContext;

        public UserRepository(IMySqlServerDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public long Add(User item)
        {
            _databaseContext.Users.Add(item);
            _databaseContext.SaveChanges();

            return item.Id;
        }

        public User Get(long id)
        {
            var usr = _databaseContext.Users.FirstOrDefault(u => u.Id == id);
            return usr;
        }

        public List<User> Get()
        {
            return _databaseContext.Users.ToList();
        }

        public IList<User> Match(ICriteria<User> criteria)
        {
            return criteria.MatchQueryFrom(_databaseContext.Users);
        }
    }
}
