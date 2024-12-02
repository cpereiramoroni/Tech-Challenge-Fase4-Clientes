using Amazon.CognitoIdentityProvider.Model;
using App.Application.Interfaces;
using App.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class ClientesControllerTests
{
    private readonly Mock<ICognitoService> _mockAuthService;
    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        _mockAuthService = new Mock<ICognitoService>();
        _controller = new ClientesController(_mockAuthService.Object);
    }

    [Fact] 
    public async Task BuscaUsuarios_ReturnsOkResult_WhenUsersFound()
    {
        // Arrange
        var cpf = "12345678900";
        var users = new List<UserType> { new UserType() };
        _mockAuthService.Setup(service => service.GetClientbyCpf(cpf)).ReturnsAsync(users);

        // Act
        var result = await _controller.BuscaUsuarios(cpf);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(users, okResult.Value);
    }
     
    [Fact]
    public async Task BuscaUsuarios_ReturnsNoContent_WhenNoUsersFound()
    {
        // Arrange
        var cpf = "12345678900";
        var users = new List<UserType>();
        _mockAuthService.Setup(service => service.GetClientbyCpf(cpf)).ReturnsAsync(users);

        // Act
        var result = await _controller.BuscaUsuarios(cpf);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletaUsuario_ReturnsOkResult()
    {
        // Arrange
        var cpf = "12345678900";
        _mockAuthService.Setup(service => service.DeleteUserByCpf(cpf)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletaUsuario(cpf);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}
