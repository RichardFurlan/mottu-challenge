using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Application.Contracts.Messaging;
using Mottu.Infrastructure.Data;
using Mottu.Infrastructure.Services.MessageBus.Consumer;
using Mottu.Infrastructure.Services.MessageBus.Publisher;

namespace Mottu.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRabbitMQ(configuration)
            .AddData(configuration);

        
        return services;
    }
    
    private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));

        return services;
    }
    
    private static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSection = configuration.GetSection("RabbitMQ"); 
        var rabbitMqOptions = new RabbitMQOptions(); 
        rabbitMqSection.Bind(rabbitMqOptions); 

        services.AddMassTransit(x =>
        {
            x.AddConsumer<MotoCreatedConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });
                
                cfg.ReceiveEndpoint("motos-created-queue", e =>
                {
                    e.ConfigureConsumer<MotoCreatedConsumer>(context);
                });
            });
        });
        
        services.AddMassTransit();
        services.AddScoped<IMotoPublisher, MotoPublisher>();
        
        return services;
    }

    
    #region Options
    public class RabbitMQOptions
    {
        public string Host { get; set; } = "localhost";
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
    

    #endregion
}