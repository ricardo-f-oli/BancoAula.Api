public interface IContaService
{
    Task<List<ContasDTO>> GetTodasAsync();
    Task<ContasDTO?> GetPorIdAsync(int id);
    Task<ContasDTO> CriarAsync(ContasDTO nova);
    Task<bool> RemoverAsync(int id);
}