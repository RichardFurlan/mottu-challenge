namespace Mottu.Domain.Entities;

public class Moto : EntityBase
{
    public Moto(int ano, string modelo, string placa)
    {
        Ano = ano;
        Modelo = modelo;
        Placa = NormalizePlaca(placa);
    }
    public int Ano { get; private set; }
    public string Modelo { get; private set; }
    public string Placa { get; private set; }
    
    public void UpdatePlaca(string placa)
    {
        Placa = NormalizePlaca(placa);
        UpdatedAt = DateTime.UtcNow;
    }
    

    private static string NormalizePlaca(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa inválida", nameof(placa));

        
        return placa.Trim().ToUpperInvariant();
    }
}