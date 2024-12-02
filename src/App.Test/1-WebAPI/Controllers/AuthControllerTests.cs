using App.Application.Interfaces;
using App.Application.ViewModels;
using App.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace App.Test._1_WebAPI.Controllers
{
    public class AuthControllerTests
    {
        public AuthController _controller;
        private readonly Mock<ICognitoService> _appServive = new();
        
        public AuthControllerTests()
        {
            _controller = new AuthController(_appServive.Object);

        }


        [Fact]
        public async Task RegisterUser_ReturnsOkResult()
        {
            // Arrange
            var user = new User("12345678900", "test@example.com", "Test User", "password123");
            _appServive.Setup(service => service.Create(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RegisterUser(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Cadastrado Com sucesso", okResult.Value);
        }

        [Fact]
        public async Task LoginUser_ReturnsOkResultWithToken()
        {
            // Arrange
            var login = new Login("12345678900", "password123");
            var token = new Token("tokenId", "accessToken", "refreshToken");
            _appServive.Setup(service => service.Login(login)).ReturnsAsync(token);

            // Act
            var result = await _controller.LoginUser(login);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(token, okResult.Value);
        }



        [Fact]
        public async Task RefreshToken_ReturnsOkResult_WithToken()
        {
            // Arrange
            var refreshToken = "sample_refresh_token";
            var expectedToken = new Token("tokenId", "accessToken", "refreshToken");
            _appServive.Setup(service => service.RefreshToken(refreshToken))
                               .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.RefreshToken(refreshToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Token>(okResult.Value);
            Assert.Equal(expectedToken, returnValue);
        }

    }
}
