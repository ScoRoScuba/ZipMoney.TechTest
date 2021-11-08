using System;
using ZipPay.API.Interfaces;

namespace ZipPay.API.Services
{
    public class AccountNumberGenerator : IAccountNumberGenerator
    {
        public string GenerateAccountNumber(long userId)
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}
