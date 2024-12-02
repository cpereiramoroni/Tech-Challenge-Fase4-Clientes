using Amazon.CognitoIdentityProvider.Model;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace App.BDD.Steps
{
    [Binding]
    public class ClienteSteps
    {
        private int clienteId;
        private IActionResult result;
        public ClientesController _controller;
        private readonly Mock<ICognitoService> _appServive = new();

        public ClienteSteps()
        {
            _controller = new ClientesController(_appServive.Object);
        }   

        [Given(@"que existe um cliente com ID (.*)")]
        public void DadoQueExisteUmClienteComID(int id)
        {
            Assert.True(id > 0);
        }

        [When(@"eu buscar o cliente")]
        public void QuandoEuBuscarOCliente()
        {

            _appServive.Setup(service => service.GetClientbyCpf(clienteId.ToString()))
                .ReturnsAsync(new List<UserType> { new UserType { Username = "XXXXXXXXXXX" } });
            // Act
            result = _controller.BuscaUsuarios(clienteId.ToString()).GetAwaiter().GetResult();

            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
        }

        [Then(@"o cliente deve existir")]
        public void EntaoOClienteDeveExistir()
        {
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var users = okResult.Value as List<UserType>;
            Assert.NotNull(users);
            Assert.True(users.Any(user => user.Username == "XXXXXXXXXXX"));
            Assert.Equal(200, okResult.StatusCode);

        }
    }
}
