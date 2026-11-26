using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoAula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly IContaService _service;

    public ContasController(IContaService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet("publico")]
    public async Task<ActionResult<List<ContasDTO>>> Get() => Ok( await _service.GetTodasAsync());
    

    /// <summary>
    /// Busca uma conta pelo identificador.
    /// </summary>
    /// <param name="id">Id da conta.</param>
    /// <response code="200">Conta encontrada.</response>
    /// <response code="404">Conta não existe.</response>
    [ProducesResponseType(typeof(ContaResponseDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContaResponseDto>> GetPorId(int id) => Ok(await _service.GetPorIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<ContasDTO>> Criar(ContasDTO nova){
        var conta = await _service.CriarAsync(nova);
        return CreatedAtAction(nameof(GetPorId), new {id = conta.Numero}, conta);
    }

    // [HttpPost("{id:int}/depositar")]
    // public ActionResult<Conta> Depositar(int id, ValorDto valor)
    // {
    //     var conta = _contas.FirstOrDefault(c => c.Id == id);
    //     if(conta == null){
    //         return NotFound();
    //     } 
    //     if(valor.Valor <=0)
    //         return BadRequest("O valor do depósito deve ser maior que zero.");
    //     conta.Saldo += valor.Valor;
    //     return Ok(conta);
    // }

    // [HttpPut("{id:int}")]
    // public IActionResult Atualizar(int id, Conta dados){
    //     var conta = _contas.FirstOrDefault(c => c.Id == id);
    //     if(conta == null){
    //         return NotFound();
    //     }
    //     conta.Tipo = dados.Tipo;
    //     conta.Saldo = dados.Saldo;
    //     conta.Ativa = dados.Ativa;
    //     return NoContent();
    // }

    [Authorize(Roles = "Gerente")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id){
        var conta = await _service.RemoverAsync(id);
        if(!conta){
            return NotFound();
        }
        return NoContent();   
    }

    [HttpPost("{id:int}/sacar")]
    public async Task<ActionResult<ContaResponseDto>> Sacar(int id, ValorDto valor) => Ok(await _service.SacarAsync(id, valor.Valor));
}
