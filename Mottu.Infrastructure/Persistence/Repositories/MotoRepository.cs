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

    public async Task<List<Moto>> GetMotosAsync(string? placa)
    {
        var query = _context.Motos
            .AsNoTracking();

        if (!string.IsNullOrEmpty(placa))
        {
            var pattern = $"%{placa.Trim()}%";
            query = query.Where(m => EF.Functions.ILike(m.Placa, pattern));
        }
                
        
        return await query.ToListAsync();
    }

    public Task<Moto?> GetMotoByIdAsync(Guid id)
    {
        return _context.Motos.SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(Moto moto)
    {
        await _context.Motos.AddAsync(moto);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}