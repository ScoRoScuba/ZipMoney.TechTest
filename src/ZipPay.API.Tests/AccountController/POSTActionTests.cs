using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using ZipPay.API.Controllers;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;
using ZipPay.API.ViewData;

namespace ZipPay.API.Tests.AccountController
{
    public class POSTActionTests
    {
        private IWebHostBuilder _webHostBuilder;
        private TestServer _testServer;
        private HttpClient _httpTestClient;

        private Mock<IAccountService> _mockAccountService;
        private Mock<ICheckCredit> _mockCreditChecker;
        private Mock<IAccountNumberGenerator> _mockAccountNumberGenerator;

        private const string UserCreateAccountRequestUrl = "/users/accounts";

        public POSTActionTests()
        {
            _mockAccountService = new Mock<IAccountService>();

            _mockCreditChecker = new Mock<ICheckCredit>(); 
            _mockCreditChecker.Setup(c => c.HasAvailableCredit(It.IsAny<long>())).Returns(true);

            _mockAccountNumberGenerator = new Mock<IAccountNumberGenerator>();
            _mockAccountNumberGenerator.Setup(g => g.GenerateAccountNumber(It.IsAny<long>()))
                .Returns(DateTime.UtcNow.Ticks.ToString());

            _mockAccountService.Setup(m => m.CreateAccount(It.IsAny<Account>())).Returns((Account account) =>
            {
                account.Id = 1;
                return account;
            });

            _webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup<UsersController>>()
                .ConfigureServices(src =>
                {
                    src.AddSingleton(_mockAccountService.Object);
                    src.AddSingleton(_mockAccountNumberGenerator.Object);
                    src.AddSingleton(_mockCreditChecker.Object);
                });

            _testServer = new TestServer(_webHostBuilder);
            _httpTestClient = _testServer.CreateClient();
        }

        [Fact]
        public void WhenPOSTCalledWithCorrectData_ReturnsHTTPStatusOKAndLocationHeader()
        {
            const string expectedLocation = @"/users/accounts/1";

            var newAccount = new AccountViewData
            {
                EmailAddress = "this@is.a.test",
            };

            var result = _httpTestClient.PostAsJsonAsync(UserCreateAccountRequestUrl, newAccount).Result;

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Headers.Location);
            Assert.Equal(expectedLocation, result.Headers.Location.OriginalString);
        }

        [Fact]
        public void WhenPOSTCalledAndUserDoesNotEnoughFundsForAvailabaleCredit_ReturnsHTTPBadRequestWithReason()
        {
            const string expectedPhrase = "User does not have available funds.";

            _mockCreditChecker.Setup(c => c.HasAvailableCredit(1)).Returns(false);

            var newAccount = new AccountViewData
            {
                EmailAddress = "this@is.a.test",
            };

            var result = _httpTestClient.PostAsJsonAsync(UserCreateAccountRequestUrl, newAccount).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }
    }
}
