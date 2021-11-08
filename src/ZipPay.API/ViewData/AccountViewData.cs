using System.ComponentModel.DataAnnotations;
using ZipPay.API.Validators;

namespace ZipPay.API.ViewData
{
    public class AccountViewData
    {
        public long Id { get; set; }

        [UserMustExistValidator]
        public long UserId { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public double CreditBalance { get; set; }
    }
}