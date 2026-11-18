public interface IContaRepository
{
    Task<List<Conta>> GetTodasAsync();
    Task<Conta> GetPorIdAsync(int id);
    Task<Conta> CriarAsync(Conta nova);
    Task<bool> RemoverAsync(int id);
}