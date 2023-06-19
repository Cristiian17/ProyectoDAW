using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Moq;
using Moq.Protected;
using Proyecto_Front_MuresanCristian.Entities;
using Proyecto_Front_MuresanCristian.Services;
using Proyecto_Front_MuresanCristian.Services.Interfaces;
using System.Net;

namespace TestUnitarios
{

    [TestClass]
    public class UnitTest1
    {
        private readonly RestService _sutLogin;
        private readonly RestService _sutAddUser;
        private string userInfoJson = "{ \"Id\" : 1, \"Email\" : \"mail@email.com\", \"UserName\" : \"name\", \"Token\" : \"token\" }";
        public UnitTest1()
        {
            var handlerLogin = HttpClientHandlerBuilder.WithResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(userInfoJson)
            });
            var handlerAddUser = HttpClientHandlerBuilder.WithResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("true")
            });
            _sutLogin = new RestService(handlerLogin);
            _sutAddUser = new RestService(handlerAddUser);
        }

        [TestMethod]
        public async Task LoginTest()
        {
            User u = new User()
            {
                Email = "mail@email.com",
                Password = "password"
            };
            var response = await _sutLogin.Login(u);
            UserInfo info = new UserInfo() { Id = 1, Email = "mail@email.com", UserName = "name", Token = "token" };
            response.Should().BeEquivalentTo(info);
        }

        [TestMethod]
        public async Task AddUserTest()
        {
            User u = new User()
            {
                Email = "mail@email.com",
                Password = "password"
            };
            var response = await _sutAddUser.AddUsser(u);
            response.Should().Be(true);
        }
    }
    public static class HttpClientHandlerBuilder
    {
        public static HttpClientHandler WithResponse(HttpResponseMessage response)
        {
            var handler = new Mock<HttpClientHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();

            return handler.Object;
        }
    }
}
