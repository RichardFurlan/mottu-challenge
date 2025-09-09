using MassTransit;
using Mottu.Application.Events;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Data;

namespace Mottu.Infrastructure.Services.MessageBus.Consumer;

public class MotoCreatedConsumer : IConsumer<MotoCreatedIntegrationEvent>
{
    private readonly AppDbContext _db;

    public MotoCreatedConsumer(AppDbContext db)
    {
        _db = db;
    }
    public async Task Consume(ConsumeContext<MotoCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        if (message.Ano.Equals(2024))
        {
            await _db.Motos2024.AddAsync(new Moto2024
            {
                Id = message.Id,
                Ano = message.Ano,
                Modelo = message.Modelo,
                Placa = message.Placa,
                CriadoEm = DateTime.UtcNow
            });
            
            await _db.SaveChangesAsync();
        }
    }
}