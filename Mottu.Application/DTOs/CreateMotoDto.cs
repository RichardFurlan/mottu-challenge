using Mottu.Domain.Entities;

namespace Mottu.Application.DTOs;

public record CreateMotoDto(int Ano, string Modelo, string Placa)
{
    public Moto ToEntity() => new Moto(Ano, Modelo, Placa);
};