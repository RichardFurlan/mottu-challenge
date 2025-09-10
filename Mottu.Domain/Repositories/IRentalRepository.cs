namespace Mottu.Domain.Repositories;

public interface IRentalRepository
{
    Task<bool> ExitsByMotoIdAsync(Guid motoId);
}