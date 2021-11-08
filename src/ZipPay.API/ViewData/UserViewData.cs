using System.ComponentModel.DataAnnotations;
using ZipPay.API.Validators;

namespace ZipPay.API.ViewData
{
    public class UserViewData
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DuplicateEmailCheck]
        public string EmailAddress { get; set; }
        [Required]
        [PositiveValueOnly]
        public double MonthlySalary { get; set; }
        [Required]
        [PositiveValueOnly]
        public double MonthlyExpenses { get; set; }
    }
}