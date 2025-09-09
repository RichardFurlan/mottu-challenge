using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Domain.Repositories;
using Mottu.Infrastructure.Data;

namespace Mottu.Infrastructure.Persistence.Repositories;

public class MotoRepository :  IMotoRepository
{
    private readonly AppDbContext _context;

    public MotoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByPlacaAsync(string placa)
    {
        return await _context.Motos.AnyAsync(m => m.Placa == placa);
    }

    public Task AddAsync(Moto moto)
    {
        _context.Motos.Add(moto);
        return _context.SaveChangesAsync();
    }
}