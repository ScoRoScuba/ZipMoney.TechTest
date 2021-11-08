using System;
using System.Collections.Generic;
using System.Net;
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

namespace ZipPay.API.Tests.UserController
{
    public class GETByIdActionTests
    {
        private IWebHostBuilder _webHostBuilder;
        private TestServer _testServer;
        private HttpClient _httpTestClient;

        private Mock<IUserService> _mockUserService;

        private const string UserRequestUrl = "/users/1";

        public GETByIdActionTests()
        {
            _mockUserService = new Mock<IUserService>();

            _mockUserService.Setup(s => s.UserEmailExists(It.IsAny<string>())).Returns(false);

            _webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup<UsersController>>()
                .ConfigureServices(src => { src.AddSingleton(_mockUserService.Object); });

            _testServer = new TestServer(_webHostBuilder);
            _httpTestClient = _testServer.CreateClient();
        }

        [Fact]
        public void WhenGETCalled_ReturnsHTTPStatusOKWithUser()
        {
            var mockUser = new User
            {
                Id = 1
            };

            _mockUserService.Setup(u => u.Get(It.IsAny<long>())).Returns(mockUser);

            var result = _httpTestClient.GetAsync(UserRequestUrl).Result;

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var data = result.Content.ReadAsStringAsync().Result;
            var theUser = JsonConvert.DeserializeObject<UserViewData>(data);

            Assert.True(mockUser.Id == theUser.Id);
        }

        [Fact]
        public void WhenGETCalledAndNoUserFoundReturnsNotFound()
        {
            _mockUserService.Setup(u => u.Get(It.IsAny<long>())).Returns((User)null);

            var result = _httpTestClient.GetAsync(UserRequestUrl).Result;

            Assert.True(!result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

    }
}
