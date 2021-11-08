using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;
using ZipPay.API.Controllers;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;
using ZipPay.API.ViewData;

namespace ZipPay.API.Tests.AccountController
{
    public class GETAction
    {
        private IWebHostBuilder _webHostBuilder;
        private TestServer _testServer;
        private HttpClient _httpTestClient;

        private Mock<IAccountService> _mockAccountService;
        private Mock<ICheckCredit> _mockCreditChecker;
        private Mock<IAccountNumberGenerator> _mockAccountNumberGenerator;

        private const string AccountRequestUrl = "/accounts";

        public GETAction()
        {
            _mockAccountService = new Mock<IAccountService>();
            _mockCreditChecker = new Mock<ICheckCredit>();
            _mockAccountNumberGenerator = new Mock<IAccountNumberGenerator>();

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
        public void GETReturnsAllAccounts()
        {
            var accounts = new List<Account>
            {
                new Account() {Id = 1, UserId = 1},
                new Account() {Id = 2, UserId = 1},
                new Account() {Id = 3, UserId = 2},
                new Account() {Id = 4, UserId = 3},
            };

            _mockAccountService.Setup(a => a.GetAccounts()).Returns(accounts);

            var result = _httpTestClient.GetAsync(AccountRequestUrl).Result;

            var data = result.Content.ReadAsStringAsync().Result;

            var theList = JsonConvert.DeserializeObject<IList<AccountViewData>>(data);

            Assert.NotEmpty(theList);

            foreach (var account in accounts)
            {
                Assert.True(theList.Any(a => a.Id == account.Id));
            }
        }

        [Fact]
        public void GETByUserIdReturnsEmptyListWhenNoAccountsForUser()
        {
            var accounts = new List<Account>();

            _mockAccountService.Setup(a => a.GetAccounts()).Returns(accounts);

            var result = _httpTestClient.GetAsync(AccountRequestUrl).Result;

            var data = result.Content.ReadAsStringAsync().Result;

            var theList = JsonConvert.DeserializeObject<IList<AccountViewData>>(data);

            Assert.Empty(theList);
        }

    }
}
