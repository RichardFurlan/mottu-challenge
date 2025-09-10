namespace Mottu.Domain.Entities;

public class Moto : EntityBase
{
    public Moto(int ano, string modelo, string placa)
    {
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
    }
    public int Ano { get; private set; }
    public string Modelo { get; private set; }
    public string Placa { get; private set; }
    
    public void UpdatePlaca(string placa)
    {
        Placa = placa;
        UpdatedAt = DateTime.Now;
    }
    

}