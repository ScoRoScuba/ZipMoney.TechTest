using System.ComponentModel.DataAnnotations;

namespace ZipPay.API.Validators
{
    public class PositiveValueOnly : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            double amount = (double) value;

            if (amount > 0.0d) return ValidationResult.Success;

            return new ValidationResult($"{validationContext.MemberName} must be greater than 0.0");
        }
    }
}
