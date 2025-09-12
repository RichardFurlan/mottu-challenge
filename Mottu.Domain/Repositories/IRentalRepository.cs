using Mottu.Domain.Entities;

namespace Mottu.Domain.Repositories;

public interface IRentalRepository
{
    Task AddAsync(Rental rental);
    Task<Rental?> GetByIdAsync(Guid id);
    Task<bool> ExistsActiveRentalByMotoIdAsync(Guid motoId);
    Task SaveAsync();
}