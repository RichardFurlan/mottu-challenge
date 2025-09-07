using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;

namespace Mottu.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<Moto> Motos { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Moto>(eb =>
        {
            eb.HasKey(m => m.Id);
            eb.Property(m => m.Modelo).IsRequired();
            eb.Property(m => m.Placa).IsRequired();
            eb.HasIndex(m => m.Placa).IsUnique(); 
        });
    }
}