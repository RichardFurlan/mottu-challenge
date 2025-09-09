using Mottu.Domain.Entities;

namespace Mottu.Application.DTOs;

public record MotoDto(Guid Id, int Ano, string Modelo, string Placa, DateTime CreatedAt, bool IsActive, bool IsDeleted)
{
    public static MotoDto FromEntity(Moto entity) => 
        new MotoDto(entity.Id, entity.Ano, entity.Modelo, entity.Placa, entity.CreatedAt, entity.IsActive, entity.IsDeleted);
};