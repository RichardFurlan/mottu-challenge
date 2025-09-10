using Microsoft.EntityFrameworkCore;
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

    public async Task<bool> ExitsByMotoIdAsync(Guid motoId)
    {
        return await _context.Rentals.AnyAsync(r => r.MotoId == motoId);
    }
}