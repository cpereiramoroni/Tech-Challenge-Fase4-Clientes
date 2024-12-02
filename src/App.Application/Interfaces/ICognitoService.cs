using Amazon.CognitoIdentityProvider.Model;
using App.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface ICognitoService
    {
        public Task<Token> Login(Login login);
        public Task Create(User user);
        public Task<Token> RefreshToken(string refreshToken);
        public Task DeleteUserByCpf(string cpf);
        public Task<List<UserType>> GetClientbyCpf(string cpf);
    }
}