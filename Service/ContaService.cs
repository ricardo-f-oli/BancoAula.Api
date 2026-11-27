public class ContaService : IContaService
{
    private readonly IContaRepository _repository;

    public ContaService(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContaResponseDto> SacarAsync(int id, decimal valor)
    {
        var conta = await _repository.GetPorIdAsync(id)
        ?? throw new KeyNotFoundException($"Conta com id {id} não encontrada.");

        if (conta.Saldo < valor)
        {
            throw new SaldoInsuficienteException("Saldo insuficiente para realizar o saque.");
        }

        conta.Saldo -= valor;
        await _repository.AtualizarAsync(conta);

        return Mapper(conta);
    }
    public async Task<ContaResponseDto?> GetPorIdAsync(int id)
    {
        var conta = await _repository.GetPorIdAsync(id)
        ?? throw new KeyNotFoundException($"Conta com id {id} não encontrada.");
        return Mapper(conta);
    }
    public async Task<List<ContasDTO>> GetTodasAsync()
    {
        var contas = await _repository.GetTodasAsync();
        return contas.Select(c => new ContasDTO(c.Numero, c.Tipo, c.Saldo, c.Ativa, c.ClienteId, c.AgenciaId)).ToList();
    }
    public async Task<ContasDTO> CriarAsync(ContasDTO nova)
    {
        var conta = new Conta
        {
            Numero = nova.Numero,
            Tipo = nova.Tipo,
            Saldo = nova.Saldo,
            Ativa = nova.Ativa,
            ClienteId = nova.ClienteId,
            AgenciaId = nova.AgenciaId
        };
        var criada = await _repository.CriarAsync(conta);
        return new ContasDTO(criada.Numero, criada.Tipo, criada.Saldo, criada.Ativa, criada.ClienteId, criada.AgenciaId);
    }
    public async Task<bool> RemoverAsync(int id)
    {
        return await _repository.RemoverAsync(id);
    }

    private static ContaResponseDto Mapper(Conta conta)
    {
        return new ContaResponseDto
        {
            Id = conta.Id,
            Numero = conta.Numero,
            Tipo = conta.Tipo,
            Saldo = conta.Saldo
        };
    }

}