namespace ZipPay.API.Interfaces
{
    public interface ICheckCredit
    {
        bool HasAvailableCredit(long userId);
    }
}