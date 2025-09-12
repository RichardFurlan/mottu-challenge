using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Domain.Repositories;
using Mottu.Infrastructure.Data;

namespace Mottu.Infrastructure.Persistence.Repositories;

public class EntregadorRepository : IEntregadorRepository
{
    private readonly AppDbContext _context;

    public EntregadorRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Entregador entregador)
    {
        await _context.Entregadores.AddAsync(entregador);
        await SaveAsync();
    }

    public async Task<Entregador?> GetByIdAsync(Guid id)
    {
        return await _context.Entregadores.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByCnpjAsync(string cnpj)
    {
        return await _context.Entregadores.AnyAsync(x => x.Cnpj == cnpj);
    }

    public Task<bool> ExistsByCnhNumberAsync(string cnhNumber)
    {
        return _context.Entregadores.AnyAsync(x => x.CnhNumber == cnhNumber);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}