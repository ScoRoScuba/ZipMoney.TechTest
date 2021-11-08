using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZipPay.API.Interfaces;

namespace ZipPay.API.Validators
{
    public class UserMustExistValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            long userId = (long) value;

            try
            {
                var userService = (IUserService)validationContext.GetService(typeof(IUserService));
                if (userService == null) Console.WriteLine("Unable to get UserService");

                var user = userService.Get(userId);

                if (user==null)
                {
                    return new ValidationResult("Unknown User");
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
