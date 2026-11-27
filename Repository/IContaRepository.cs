public interface IContaRepository
{
    Task<List<Conta>> GetTodasAsync();
    Task<Conta?> GetPorIdAsync(int id);
    Task<Conta> CriarAsync(Conta nova);
    Task AtualizarAsync(Conta conta);
    Task<bool> RemoverAsync(int id);
    Task<List<Transacao>> GetTransacoesPorContaIdAsync(int id);
}