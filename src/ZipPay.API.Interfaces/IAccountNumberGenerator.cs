namespace ZipPay.API.Interfaces
{
    public interface IAccountNumberGenerator
    {
        string GenerateAccountNumber(long userId);
    }
}