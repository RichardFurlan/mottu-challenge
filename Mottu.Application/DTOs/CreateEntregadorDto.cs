using Mottu.Domain.Entities;

namespace Mottu.Application.DTOs;

public record CreateEntregadorDto(
    string Nome,
    string Cnpj,
    DateTime DataNascimento,
    string CnhNumber,
    string CnhType
)
{
    public Entregador ToEntity()
        => new Entregador(Nome, Cnpj, DataNascimento, CnhNumber, CnhType);
}
    