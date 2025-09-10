using Mottu.Domain.Entities;

namespace Mottu.Domain.Repositories;

public interface IMotoRepository
{
    Task<bool> ExistsByPlacaAsync(string placa);
    Task<List<Moto>> GetMotosAsync(string? placa);
    Task<Moto?> GetMotoByIdAsync(Guid id);
    Task AddAsync(Moto moto);
    
    Task SaveAsync();
}