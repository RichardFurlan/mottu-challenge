namespace Mottu.Domain.Entities;

public class Moto : EntityBase
{
    public Moto(int ano, string modelo, string placa)
    {
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
    }
    public int Ano { get; set; }
    public string Modelo { get; set; }
    public string Placa { get; set; }
}