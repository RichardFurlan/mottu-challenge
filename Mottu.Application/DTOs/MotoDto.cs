namespace Mottu.Application.DTOs;

public record MotoDto(Guid Id, int Ano, string Modelo, string Placa, DateTime CreatedAt);