using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Domain.Repositories;
using Mottu.Infrastructure.Data;

namespace Mottu.Infrastructure.Persistence.Repositories;

public class RentalRepository :  IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Rental rental)
    {
        await _context.Rentals.AddAsync(rental);
        await SaveAsync();
    }

    public async Task<Rental?> GetByIdAsync(Guid id)
    {
        return await _context.Rentals.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsActiveRentalByMotoIdAsync(Guid motoId)
    {
        return await _context.Rentals.AnyAsync(r => r.MotoId == motoId && r.ActualReturnDate == null);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}