using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoAula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly BancoContext _db;
    public ContasController(BancoContext db) =>_db = db;

    public record ValorDto(decimal Valor);
    private static int _proximoId = 3;

    [HttpGet]
    public ActionResult<List<Conta>> Get() => Ok( _db.Contas.ToList());
    

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Conta>> GetPorId(int id){
        var conta = await _db.Contas.FindAsync(id);
        if(conta == null){
            return NotFound();
        }
        return Ok(conta);
    }

    [HttpPost]
    public async Task<ActionResult<Conta>> Criar(Conta nova){
        _db.Contas.Add(nova);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPorId), new {id = nova.Id}, nova);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id){
        var conta = await _db.Contas.FindAsync(id);
        if(conta == null){
            return NotFound();
        }
        _db.Contas.Remove(conta);
        await _db.SaveChangesAsync();
        return NoContent();   
    }
    [HttpGet("clientes/{clienteId:int}/emprestimos")]
    public async Task<ActionResult<List<Emprestimo>>> GetEmprestimosPorCliente(int clienteId)
    {
        var cliente = await _db.Clientes.FindAsync(clienteId);
        if (cliente == null)
        {
            return NotFound();
        }
        var emprestimos = await _db.Emprestimos.Where(e => e.ClienteId == cliente.Id).ToListAsync();
        return Ok(emprestimos);
    } 
}
