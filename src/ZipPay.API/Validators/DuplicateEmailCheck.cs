using System;
using System.ComponentModel.DataAnnotations;
using ZipPay.API.Interfaces;

namespace ZipPay.API.Validators
{
    public class DuplicateEmailCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string emailAddressToCheck = (string) value;

            try
            {
                var userService = (IUserService) validationContext.GetService(typeof(IUserService));
                if(userService== null) Console.WriteLine("Unable to get UserService");

                if (userService.UserEmailExists(emailAddressToCheck))
                {
                    return new ValidationResult("The EmailAddress has already been used");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION : Unable to Get UserService - {ex.Message}");
            }

            return ValidationResult.Success;
        }
    }
}
