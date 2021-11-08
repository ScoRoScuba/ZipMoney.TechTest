namespace ZipPay.API.Model
{
    public class Account
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string EmailAddress { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public double CreditBalance { get; set; }
    }
}