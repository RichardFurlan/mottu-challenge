using Microsoft.Extensions.DependencyInjection;
using Mottu.Application.Services;

namespace Mottu.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMotoService, MotoService>();
        services.AddScoped<IEntregadorService, EntregadorService>();
        services.AddScoped<IRentalService, RentalService>();
        return services;
    }
}