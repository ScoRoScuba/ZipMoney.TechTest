using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;
using ZipPay.API.ViewData;

namespace ZipPay.API.Controllers
{

    [ApiController]
    [Produces("application/json")]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICheckCredit _creditChecker;
        private IAccountNumberGenerator _accountNumberGenerator;

        public AccountController(IAccountService accountService, ICheckCredit creditChecker, IAccountNumberGenerator accountNumberGenerator)
        {
            _accountService = accountService;
            _creditChecker = creditChecker;
            _accountNumberGenerator = accountNumberGenerator;
        }

        [HttpPost]
        [Route("/users/accounts")]
        public IActionResult Post(AccountViewData accountViewData)
        {
            if (!_creditChecker.HasAvailableCredit(accountViewData.UserId))
            {
                return BadRequest(new ValidationResult("User does not have available funds.", new []{"availableCredit"}));
            }

            var newAccount = new Account
            {
                UserId = accountViewData.UserId,
                EmailAddress = accountViewData.EmailAddress,
                AccountNumber = string.IsNullOrWhiteSpace(accountViewData.AccountNumber) ? _accountNumberGenerator.GenerateAccountNumber(accountViewData.UserId) : accountViewData.AccountNumber,
                AccountName = accountViewData.AccountName,
            };

            var result = _accountService.CreateAccount(newAccount);

            accountViewData.Id = result.Id;

            return Created($@"/users/accounts/{result.Id}", accountViewData);
        }

        [HttpGet]
        [Route("/users/{userId}/accounts")]
        public IActionResult Get(long userId)
        {
            var result = _accountService.GetUsersAccounts(userId);

            var data = result.Select((item, index) => new AccountViewData
            {
                AccountName = item.AccountName,
                AccountNumber = item.AccountNumber,
                CreditBalance = item.CreditBalance,
                EmailAddress = item.EmailAddress,
                Id = item.Id,
                UserId = item.UserId
            });

            return Ok(data);
        }

        [HttpGet]
        [Route("/accounts")]
        public IActionResult Get()
        {
            var result = _accountService.GetAccounts();

            var data = result.Select((item, index) => new AccountViewData
            {
                AccountName = item.AccountName,
                AccountNumber = item.AccountNumber,
                CreditBalance = item.CreditBalance,
                EmailAddress = item.EmailAddress,
                Id = item.Id,
                UserId = item.UserId
            });

            return Ok(data);
        }



    }
}
