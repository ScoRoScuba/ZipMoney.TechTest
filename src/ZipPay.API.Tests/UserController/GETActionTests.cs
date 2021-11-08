using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
    public class GETActionTests
    {
        private IWebHostBuilder _webHostBuilder;
        private TestServer _testServer;
        private HttpClient _httpTestClient;

        private Mock<IUserService> _mockUserService;

        private const string UserRequestUrl = "/users";

        public GETActionTests()
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
        public void WhenGETCalled_ReturnsHTTPStatusOK()
        {
            var mockUserList = new List<User>
            {
                new User {Id = 1},
                new User {Id = 2}
            };

            _mockUserService.Setup(u => u.GetAll()).Returns(mockUserList);

            var result = _httpTestClient.GetAsync(UserRequestUrl).Result;

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void WhenGETCalled_ReturnsAllUsers()
        {
            var mockUsers = new List<User>
            {
                new User {Id = 1},
                new User {Id = 2},
            };

            _mockUserService.Setup(s => s.GetAll()).Returns(mockUsers);

            var result = _httpTestClient.GetAsync(UserRequestUrl).Result;

            Assert.True(result.IsSuccessStatusCode);

            var data = result.Content.ReadAsStringAsync().Result;

            var theList = JsonConvert.DeserializeObject<IList<UserViewData>>(data);

            Assert.True(theList.Count == 2);

            foreach (var usr in mockUsers)
            {
                Assert.Contains(theList, u => u.Id == usr.Id);
            }
        }

        [Fact]
        public void WhenGETCalledAndNoUsersReturnsEmptyList()
        {
            var mockUsers = new List<User>();

            _mockUserService.Setup(s => s.GetAll()).Returns(mockUsers);

            var result = _httpTestClient.GetAsync(UserRequestUrl).Result;

            Assert.True(result.IsSuccessStatusCode);

            var data = result.Content.ReadAsStringAsync().Result;

            var theList = JsonConvert.DeserializeObject<IList<UserViewData>>(data);

            Assert.Empty(theList);            
        }
    }
}
