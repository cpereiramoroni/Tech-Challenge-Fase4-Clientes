using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using App.Application.CustomExceptions;
using App.Application.Interfaces;
using App.Application.Utils;
using App.Application.Validations;
using App.Application.ViewModels;
using AWS_lambda_Auth.Validations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace App.Application.Services
{

    [ExcludeFromCodeCoverage]
    public class CognitoService : ICognitoService
    {
        private readonly AmazonCognitoIdentityProviderClient _cognitoClient;
        private readonly string _clientId;
        private readonly string _userPoolId;
        private readonly string _keyId;
        private readonly string _keyIdSecret;
        private readonly string _clientSecret;

        // Construtor para desenvolvimento
        public CognitoService(IConfiguration config)
        {
            _clientId = config["CognitoClientId"] ?? "";
            _userPoolId = config["CognitoUserPoolId"] ?? "";
            _clientSecret = config["CognitoClientSecret"] ?? "";

#if DEBUG
            // Em ambiente de desenvolvimento, usar AWS credentials explícitas
            _keyId = config["AWS_ACCESS_KEY_ID"] ?? "";
            _keyIdSecret = config["AWS_SECRET_ACCESS_KEY"] ?? "";
            var credentials = new BasicAWSCredentials(_keyId, _keyIdSecret);
            _cognitoClient = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.USEast1);
            

#else
        // Em outros ambientes, usar a configuração padrão (credenciais automáticas)
        _cognitoClient = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1);
#endif
        }

        public dynamic GetChaves()
        {
            return new
            {
                _clientId = _clientId,
                _userPoolId = _userPoolId,
                _keyId = _keyId,
                _keyIdSecret = _keyIdSecret
            };

        }

        public async Task Create(User user)
        {

            // Calcular o SECRET_HASH
            var secretHash = CognitoUtils.CalculateSecretHash(_clientId, _clientSecret, user.Cpf);


            var rtnUserValid = new UsuarioValidation().Validate(user);
            if (!rtnUserValid.IsValid)
                throw new BadRequestException(rtnUserValid.Errors.Select(e => e.ErrorMessage).ToList());

            // Verificar se o usuário já existe usando ListUsersAsync
            var userExists = await UserExistsAsync(user.Cpf);
            if (userExists)
                throw new UserExistException();

            // Se o usuário não existe, prosseguir com o registro
            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = secretHash,
                Username = Utils.Validator.LimparCpf(user.Cpf),
                Password = user.Password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType
                    {
                        Name = "name", // Atributo para o nome
                        Value = user.Nome
                    },
                    new AttributeType
                    {
                        Name = "email", // Atributo para o e-mail
                        Value = user.Email
                    }
                }

            };

            await _cognitoClient.SignUpAsync(signUpRequest);

            var confirmRequest = new AdminConfirmSignUpRequest
            {
                Username = Utils.Validator.LimparCpf(user.Cpf),
                UserPoolId = _userPoolId,
                
            };

            await _cognitoClient.AdminConfirmSignUpAsync(confirmRequest);

        }




        public async Task<Token> Login(Login login)
        {
            var rtnloginValid = new LoginValidation().Validate(login);
            if (!rtnloginValid.IsValid)
                throw new BadRequestException(rtnloginValid.Errors.Select(e => e.ErrorMessage).ToList());

            var authRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = _userPoolId,
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", Utils.Validator.LimparCpf(login.Cpf) },
                { "PASSWORD", login.Password }
            }
            };




            var authResponse = await TryInitiateAuth(authRequest);
            return new Token(
                authResponse.AuthenticationResult.IdToken,
                authResponse.AuthenticationResult.AccessToken,
                authResponse.AuthenticationResult.RefreshToken);
        }

        private async Task<AdminInitiateAuthResponse> TryInitiateAuth(AdminInitiateAuthRequest authRequest)
        {
            try
            {
                return await _cognitoClient.AdminInitiateAuthAsync(authRequest);
            }
            catch(Exception ex)
            {
                throw new ClientException("cpf ou senha invalidos");
            }
        }


        public async Task<Token> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ClientException("Refresh token is required");

            var refreshRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = _userPoolId,
                ClientId = _clientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                AuthParameters = new Dictionary<string, string>
            {
                { "REFRESH_TOKEN", refreshToken }
            }
            };

            var refreshResponse = await _cognitoClient.AdminInitiateAuthAsync(refreshRequest);
            return new Token(
                refreshResponse.AuthenticationResult.IdToken,
                refreshResponse.AuthenticationResult.AccessToken,
                refreshResponse.AuthenticationResult.RefreshToken);
            // Retornar o refresh token original se não for renovado
        }

        private async Task<bool> UserExistsAsync(string username)
        {
            var rtn =await GetClientbyCpf(username);
            // Se encontrar um ou mais usuários, significa que o usuário já existe
            return rtn.Count > 0;
        }

        public async Task<List<UserType>> GetClientbyCpf(string cpf)
        {
            var listUsersRequest = new ListUsersRequest
            {
                UserPoolId = _userPoolId,

                Filter = $"username = \"{cpf}\"" // Filtro pelo nome de usuário
            };

            var listUsersResponse = await _cognitoClient.ListUsersAsync(listUsersRequest);

            
            return listUsersResponse.Users;
        }
        public async Task DeleteUserByCpf(string cpf)
        {
            var userExists = await UserExistsAsync(cpf);
            if (!userExists)
                throw new ClientException("User not exist");
            

            var deleteUserRequest = new AdminDeleteUserRequest
            {
                UserPoolId = _userPoolId,
                Username = Utils.Validator.LimparCpf(cpf)
            };

            await _cognitoClient.AdminDeleteUserAsync(deleteUserRequest);
        }
    }

}
