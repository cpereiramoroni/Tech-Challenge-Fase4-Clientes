using System;
using System.Collections.Generic;
using App.Application.CustomExceptions;
using Xunit;

namespace App.Application.Tests.Exceptions
{
    public class ClientExceptionTests
    {
        [Fact]
        public void ClientException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var message = "Test message";

            // Act
            var exception = new ClientException(message);

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void ClientException_WithMessageAndInnerException_ShouldSetProperties()
        {
            // Arrange
            var message = "Test message";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new ClientException(message, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void UserExistException_ShouldSetDefaultMessage()
        {
            // Act
            var exception = new UserExistException();

            // Assert
            Assert.Equal("cliente ja existe", exception.Message);
        }

        [Fact]
        public void BadRequestException_WithErrors_ShouldSetProperties()
        {
            // Arrange
            var errors = new List<string> { "Error1", "Error2" };

            // Act
            var exception = new BadRequestException(errors);

            // Assert
            Assert.Equal("Houve erros na validação dos dados.", exception.Message);
            Assert.Equal(errors, exception.Errors);
        }
    }
}
