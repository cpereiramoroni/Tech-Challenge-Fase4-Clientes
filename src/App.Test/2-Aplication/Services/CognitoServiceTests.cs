using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using APICliente.Tests;
using App.Application.CustomExceptions;
using App.Application.Services;
using App.Application.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace App.Application.Tests.Services
{
    public class CognitoServiceTests
    {
        private readonly IConfiguration _mockConfig;
        private readonly Mock<AmazonCognitoIdentityProviderClient> _mockCognitoClient;
        private readonly CognitoService _cognitoService;

        public CognitoServiceTests()
        {
            _mockConfig = new MoroniWebApplication().MyConfiguration;
            _mockCognitoClient = new Mock<AmazonCognitoIdentityProviderClient>(MockBehavior.Strict, new BasicAWSCredentials("accessKey", "secretKey"), Amazon.RegionEndpoint.USEast1);
            _cognitoService = new CognitoService(_mockConfig);
        }

       

        [Fact]
        public async Task Create_ShouldThrowUserExistException_WhenUserAlreadyExists()
        {
            // Arrange
            var user = new User("123456789", "test@example.com", "Test User", "password");
            _mockCognitoClient.Setup(client => client.ListUsersAsync(It.IsAny<ListUsersRequest>(), default))
                .ReturnsAsync(new ListUsersResponse { Users = new List<UserType> { new UserType() } });

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _cognitoService.Create(user));
        }


       
    }
}
