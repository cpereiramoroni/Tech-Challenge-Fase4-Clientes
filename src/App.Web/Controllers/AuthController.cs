using App.Application.Interfaces;
using App.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;



[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{

    private readonly ICognitoService _authService;

    public AuthController(ICognitoService authService)
    {
        _authService = authService;
    }

    [HttpPost("cadastro")]
    [SwaggerOperation(
           Summary = "EndPoint para Cadastrar um usuario no Cognito via api-gateway e lambda",
           Description = @" </br>
                          <b>Parâmetros de entrada:</b>
                        <br/> &bull; <b>CPF</b>:cpf do usuario &rArr; <font color='red'><b>Obrigatorio</b></font>                              
                        <br/> &bull; <b>Password</b>:cpf do usuario &rArr; <font color='red'><b>Obrigatorio</b></font>                              
                        <br/> &bull; <b>Nome</b>:nome Usuarioa&rArr; <font color='green'><b>Opcional</b></font>                              
                        <br/> &bull; <b>Email</b>:Email Usuario &rArr; <font color='green'><b>Opcional</b></font>"
                        ,
            Tags = ["Cadastrar"]
         )]

    [SwaggerResponse(200, "Criado com sucesso!")]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe!", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!", null)]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!", null)]

    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        await _authService.Create(user);
        return Ok("Cadastrado Com sucesso");
    }
    [HttpPost("login")]
    [SwaggerOperation(
           Summary = "EndPoint para gerar token via api-gateway e lambda",
           Description = @" </br>
                          <b>Parâmetros de entrada:</b>
                        <br/> &bull; <b>CPF</b>:cpf do usuario &rArr; <font color='red'><b>Obrigatorio</b></font>                              
                        <br/> &bull; <b>Password</b>:cpf do usuario &rArr; <font color='red'><b>Obrigatorio</b></font> ",
            Tags = ["Login"]
         )]

    [SwaggerResponse(200, "Token criado com sucesso!", typeof(Token))]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe!", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!", null)]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!", null)]

    public async Task<IActionResult> LoginUser([FromBody] Login login)
    {
        var user = await _authService.Login(login);
        return Ok(user);
    }

    [HttpPost("refreshtoken")]
    [SwaggerOperation(
           Summary = "EndPoint atualizar token no Cognito via api-gateway e lambda",
           Description = @" </br>
                          <b>Parâmetros de entrada:</b>
                         <br/> &bull; <b>Token</b>:token anteriormente gerado no login &rArr; <font color='red'><b>Obrigatorio</b></font>   ",
                    Tags = ["Login"]
         )]

    [SwaggerResponse(200, "Token criado com sucesso!", typeof(Token))]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe!", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!", null)]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!", null)]

    public async Task<IActionResult> RefreshToken([FromBody] string refreshtoken)
    {
        var token = await _authService.RefreshToken(refreshtoken);
        return Ok(token);
    }


   

}

