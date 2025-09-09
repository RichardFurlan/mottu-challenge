namespace Mottu.Application.Events;

public record MotoCreatedIntegrationEvent(
    Guid Id,
    int Ano,
    string Modelo,
    string Placa
);