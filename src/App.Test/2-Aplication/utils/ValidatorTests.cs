using App.Application.Utils;
using Xunit;

namespace App.Application.Tests.Utils
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("555", false)]
        [InlineData("75544736030", true)]
        [InlineData("111", false)]
        public void CpfIsValid_ShouldValidateCpfCorrectly(string cpf, bool expected)
        {
            bool result = Validator.CpfIsValid(cpf);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("another.test@domain.co", true)]
        public void ValidarEmail_ShouldValidateEmailCorrectly(string email, bool expected)
        {
            bool result = Validator.ValidarEmail(email);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123.456.789-09", "12345678909")]
        [InlineData("111.444.777-35", "11144477735")]
        [InlineData("000.000.000-00", "00000000000")]
        public void LimparCpf_ShouldRemoveNonNumericCharacters(string cpf, string expected)
        {
            string result = Validator.LimparCpf(cpf);
            Assert.Equal(expected, result);
        }
    }
}
