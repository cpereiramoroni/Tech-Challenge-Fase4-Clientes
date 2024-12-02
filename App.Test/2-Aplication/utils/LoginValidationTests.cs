using App.Application.Validations;
using App.Application.ViewModels;
using FluentValidation.TestHelper;
using Xunit;

namespace App.Application.Tests.Validations
{
    public class LoginValidationTests
    {
        private readonly LoginValidation _validator;

        public LoginValidationTests()
        {
            _validator = new LoginValidation();
        }

        [Fact]
        public void Should_Have_Error_When_Cpf_Is_Null()
        {
            var model = new Login(null, "password");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Cpf).WithErrorMessage("CPF é obrigatorio.");
        }

        [Fact]
        public void Should_Have_Error_When_Cpf_Is_Empty()
        {
            var model = new Login(string.Empty, "password");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Cpf).WithErrorMessage("CPF é obrigatorio.");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Null()
        {
            var model = new Login("123456789", null);
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Password é obrigatorio.");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var model = new Login("123456789", string.Empty);
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Password é obrigatorio.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Cpf_And_Password_Are_Provided()
        {
            var model = new Login("123456789", "password");
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Cpf);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
