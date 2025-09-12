using Mottu.Domain.Entities;

namespace Mottu.Domain.Repositories;

public interface IEntregadorRepository
{
    Task AddAsync(Entregador entregador);
    Task<Entregador?> GetByIdAsync(Guid id);
    Task<bool> ExistsByCnpjAsync(string cnpj);
    Task<bool> ExistsByCnhNumberAsync(string cnhNumber);
    Task SaveAsync();
}