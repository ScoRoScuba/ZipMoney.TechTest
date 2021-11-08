using System.Collections.Generic;
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

namespace ZipPay.API.Tests.UserController
{
    public class POSTActionTests
    {
        private IWebHostBuilder _webHostBuilder;
        private TestServer _testServer;
        private HttpClient _httpTestClient;

        private Mock<IUserService> _mockUserService;

        private const string UserRequestUrl = "/users";

        public POSTActionTests()
        {
            _mockUserService = new Mock<IUserService>();

            _mockUserService.Setup(m => m.CreateUser(It.IsAny<User>())).Returns((User user) =>
            {
                user.Id = 1;
                return user;
            });

            _mockUserService.Setup(s => s.UserEmailExists(It.IsAny<string>())).Returns(false);

            _webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup<UsersController>>()
                .ConfigureServices(src => { src.AddSingleton(_mockUserService.Object); });

            _testServer = new TestServer(_webHostBuilder);
            _httpTestClient = _testServer.CreateClient();
        }

        [Fact]
        public void WhenPOSTCalledWithCorrectData_ReturnsHTTPStatusOKAndLocationHeader()
        {
            const string expectedLocation = @"/users/1";

            var newUser = new UserViewData
            {
                Name = "This is a test",
                EmailAddress = "this@is.a.test",
                MonthlySalary = 1500d,
                MonthlyExpenses = 900d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, newUser).Result;

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Headers.Location);
            Assert.Equal(expectedLocation, result.Headers.Location.OriginalString);
        }

        [Fact]
        public void WhenPOSTCalledWithNullData_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "A non-empty request body is required.";

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, (UserViewData) null).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithNoName_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "The Name field is required.";

            var payload = new UserViewData
            {
                EmailAddress = "email@address.com",
                MonthlySalary = 1.0d,
                MonthlyExpenses = 1.0d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithNoEmail_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "The EmailAddress field is required.";

            var payload = new UserViewData
            {
                Name = "A Name",
                MonthlySalary = 1.0d,
                MonthlyExpenses = 1.0d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithDuplicateEmails_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "The EmailAddress has already been used";

            const string emailAddress = "this@is.a.duplicate";

            _mockUserService.Setup(s => s.UserEmailExists(emailAddress)).Returns(true);

            var payload = new UserViewData
            {
                EmailAddress = emailAddress,
                Name = "A Name",
                MonthlySalary = 1.0d,
                MonthlyExpenses = 1.0d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithNoSalary_ReturnsHTTPStatusBadRequest()
        {
            const string expectedPhrase = "MonthlySalary must be greater than 0.0";

            var payload = new UserViewData
            {
                Name = "A Name",
                EmailAddress = "email@address.com",
                MonthlyExpenses = 1.0d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithNegativeSalary_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "MonthlySalary must be greater than 0.0";

            var payload = new UserViewData
            {
                MonthlySalary = -10.00d,
                Name = "A Name",
                EmailAddress = "email@address.com",
                MonthlyExpenses = 1.0d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithNoMonthlyExpenses_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "MonthlyExpenses must be greater than 0.0";

            var payload = new UserViewData
            {
                MonthlySalary = 10.00d,
                Name = "A Name",
                EmailAddress = "email@address.com",
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithNegativeMonthlyExpenses_ReturnsHTTPStatusBadRequestWithReason()
        {
            const string expectedPhrase = "MonthlyExpenses must be greater than 0.0";

            var payload = new UserViewData
            {
                MonthlySalary = 10.00d,
                Name = "A Name",
                EmailAddress = "email@address.com",
                MonthlyExpenses = -11.0d
            };

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.True(content.Contains(expectedPhrase));
        }

        [Fact]
        public void WhenPOSTCalledWithEmptyViewData_ReturnsHTTPStatusBadRequestWithReasons()
        {
            var expectedPhrases = new List<string>
            {
                "The Name field is required.",
                "The EmailAddress field is required.",
                "MonthlySalary must be greater than 0.0",
                "MonthlyExpenses must be greater than 0.0"
            };

            var payload = new UserViewData();

            var result = _httpTestClient.PostAsJsonAsync(UserRequestUrl, payload).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            var content = result.Content.ReadAsStringAsync().Result;

            foreach (var expectedPhrase in expectedPhrases)
            {
                Assert.True(content.Contains(expectedPhrase));
            }
        }
    }
}
