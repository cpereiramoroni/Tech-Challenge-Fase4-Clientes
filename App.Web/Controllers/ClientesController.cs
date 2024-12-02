using App.Application.Interfaces;
using App.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;



[ApiController]
[Route("api/[controller]")]

public class ClientesController : ControllerBase
{

    private readonly ICognitoService _authService;

    public ClientesController(ICognitoService authService)
    {
        _authService = authService;
    }

    [HttpGet("{cpf}")]
    [SwaggerOperation(
           Summary = "EndPoint para buscar um usuario no Cognito via api-gateway e lambda",
           Description = @" </br>
                          <b>Parâmetros de entrada:</b>
                        <br/> &bull; <b>CPF</b>:cpf do usuario &rArr; <font color='red'><b>Obrigatorio</b></font>"
                        ,
            Tags = ["Clientes"]
         )]

    [SwaggerResponse(200, "Criado com sucesso!")]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe!", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!", null)]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!", null)]

    public async Task<IActionResult> BuscaUsuarios([FromRoute] string cpf)
    {
        var rtn=await _authService.GetClientbyCpf(cpf);
        if (rtn.Count == 0)
            return NoContent();
        return Ok(rtn);
    }

    [HttpDelete("{cpf}")]
    [SwaggerOperation(
       Summary = "EndPoint para deletar um usuario no Cognito via api-gateway e lambda",
       Description = @" </br>
                          <b>Parâmetros de entrada:</b>
                        <br/> &bull; <b>CPF</b>:cpf do usuario &rArr; <font color='red'><b>Obrigatorio</b></font>"
                    ,
        Tags = ["Clientes"]
     )]

    [SwaggerResponse(200, "Deletado com sucesso!")]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe!", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!", null)]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!", null)]

    public async Task<IActionResult> DeletaUsuario([FromRoute] string cpf)
    {
        await _authService.DeleteUserByCpf(cpf);
        return Ok();
    }

}

