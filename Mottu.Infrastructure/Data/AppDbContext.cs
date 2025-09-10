using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;

namespace Mottu.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Moto2024> Motos2024 { get; set; }
    public DbSet<Rental> Rentals { get; set; } = null!;
    public DbSet<Entregador> Entregadores { get; set; } = null!;
    
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
        
        modelBuilder.Entity<Entregador>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Nome).IsRequired();
            eb.Property(e => e.Cnpj).IsRequired();
            eb.HasIndex(e => e.Cnpj).IsUnique();
            eb.Property(e => e.CnhNumber).IsRequired();
            eb.HasIndex(e => e.CnhNumber).IsUnique();
        });
        
        modelBuilder.Entity<Rental>(eb =>
        {
            eb.HasKey(r => r.Id);
            eb.HasOne(r => r.Moto)
                .WithMany() 
                .HasForeignKey(r => r.MotoId)
                .OnDelete(DeleteBehavior.Restrict); 
            eb.HasOne(r => r.Entregador)
                .WithMany()
                .HasForeignKey(r => r.EntregadorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Moto2024>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Placa).IsRequired();
        });
    }
}