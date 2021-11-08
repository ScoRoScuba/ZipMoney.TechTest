using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;
using ZipPay.API.ViewData;

namespace ZipPay.API.Controllers
{
    [ApiController]
    [Produces("application/json")]    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController( IUserService userService )
        {
            _userService = userService;
        }
        
        [HttpPost]
        [Route("/users")]
        public IActionResult Post(UserViewData userViewData)
        {
            var user = new User
            {
                Id = userViewData.Id,
                Name = userViewData.Name,
                EmailAddress = userViewData.EmailAddress,
                MonthlyExpenses = userViewData.MonthlyExpenses,
                MonthlySalary = userViewData.MonthlySalary
            };
                        
            var result = _userService.CreateUser(user);

            userViewData.Id = result.Id;

            return Created($@"/users/{result.Id}", userViewData);
        }

        [HttpGet]
        [Route("/users")]
        public IActionResult Get()
        {
            var result = _userService.GetAll();

            var userViewModels = result.Select((user, index) => new UserViewData
            {
                Id = user.Id,
                Name = user.Name,
                EmailAddress = user.EmailAddress,
                MonthlyExpenses = user.MonthlyExpenses,
                MonthlySalary = user.MonthlySalary
            });

            return Ok(userViewModels);
        }

        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult Get(long userId)
        {
            var user = _userService.Get(userId);

            if (user == null)  return NotFound();
            
            var userViewModel = new UserViewData
            {
                Id = user.Id,
                Name = user.Name,
                EmailAddress = user.EmailAddress,
                MonthlyExpenses = user.MonthlyExpenses,
                MonthlySalary = user.MonthlySalary
            };

            return Ok(userViewModel);
        }

    }
}

