using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;
using ZipPay.API.Repository;

namespace ZipPay.API.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User CreateUser(User user)
        {
            var result = _userRepository.Add(user);
            
            return user;
        }

        public bool UserEmailExists(string emailAddress)
        {
            var result = _userRepository.Match(new FindEmailCriteria(emailAddress));

            return EnumerableExtensions.Any(result);
        }

        public IList<User> GetAll()
        {
            return _userRepository.Get();
        }

        public User Get(long userId)
        {
            var result = _userRepository.Match(new FindUserByIdCriteria(userId));

            return Enumerable.FirstOrDefault(result);
        }

        public User Get(string emailAddress)
        {
            var result = _userRepository.Match(new FindUserByEmailCriteria(emailAddress));

            return Enumerable.FirstOrDefault(result);
        }
    }
}
