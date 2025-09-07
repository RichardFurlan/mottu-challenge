namespace Mottu.Domain.Entities;

public class Moto : EntityBase
{
    public int Ano { get; set; }
    public string Modelo { get; set; } = null!;
    public string Placa { get; set; } = null!;
}