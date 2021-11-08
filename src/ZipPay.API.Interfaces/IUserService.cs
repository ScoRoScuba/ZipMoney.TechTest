using System.Collections.Generic;
using ZipPay.API.Model;

namespace ZipPay.API.Interfaces
{
    public interface IUserService
    {
        User CreateUser(User user);
        bool UserEmailExists(string emailAddress);
        IList<User> GetAll();
        User Get(long userId);
        User Get(string emailAddress);
    }
}
