using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Application.Contracts.Messaging;
using Mottu.Application.Contracts.Storage;
using Mottu.Domain.Repositories;
using Mottu.Infrastructure.Data;
using Mottu.Infrastructure.Persistence.Repositories;
using Mottu.Infrastructure.Services.MessageBus.Consumer;
using Mottu.Infrastructure.Services.MessageBus.Publisher;
using Mottu.Infrastructure.Services.Storage;

namespace Mottu.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddData(configuration)
            .AddRepository()
            .AddRabbitMq(configuration)
            .AddMinIo(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("MottuDb")));
        return services;
    }
    
    private static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IMotoRepository, MotoRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();        
        services.AddScoped<IEntregadorRepository, EntregadorRepository>();
        services.AddSingleton<IStorageService, MinioStorageService>();
        return services;
    }
    
    private static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
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
        
        services.AddScoped<IMotoPublisher, MotoPublisher>();
        
        return services;
    }

    private static IServiceCollection AddMinIo(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection("Minio"));
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