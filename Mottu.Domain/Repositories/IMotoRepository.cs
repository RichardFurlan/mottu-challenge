using Mottu.Domain.Entities;

namespace Mottu.Domain.Repositories;

public interface IMotoRepository
{
    Task<bool> ExistsByPlacaAsync(string placa);
    Task AddAsync(Moto moto);
}