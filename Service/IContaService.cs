public interface IContaService
{
    Task<List<ContasDTO>> GetTodasAsync();
    Task<ContaResponseDto?> GetPorIdAsync(int id);
    Task<ContasDTO> CriarAsync(ContasDTO nova);
    Task<bool> RemoverAsync(int id);
    Task<ContaResponseDto> SacarAsync(int id, decimal valor);
    Task<ContaResponseDto> DepositarAsync(int id, decimal valor);
    Task<List<Transacao>> GetTransacoesPorContaIdAsync(int id);
}