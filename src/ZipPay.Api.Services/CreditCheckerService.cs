using ZipPay.API.Interfaces;

namespace ZipPay.API.Services
{
    public class CreditCheckerService : ICheckCredit
    {
        private readonly IUserService _userService;

        public CreditCheckerService(IUserService userService)
        {
            _userService = userService;
        }

        public bool HasAvailableCredit(long userId)
        {
            var user = _userService.Get(userId);

            var available = user.MonthlySalary - user.MonthlyExpenses;
                
            if (available < 1000.00)
            {
                return false;
            }

            return true;
        }
    }
}
