namespace Mottu.Domain.Entities;

public class Entregador : EntityBase
{
    
    public Entregador(string nome, string cnpj, DateTime dataNascimento, string cnhNumber, string cnhType, string? cnhImageUrl = null)
    {
        Nome = nome;
        Cnpj = cnpj;
        DataNascimento = dataNascimento;
        CnhNumber = cnhNumber;
        CnhType = cnhType;
        CnhImageUrl = cnhImageUrl;
    }
    public string Nome { get; private set; }
    public string Cnpj { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string CnhNumber { get; private set; }
    public string CnhType { get; private set; } // "A","B","A+B"
    public string? CnhImageUrl { get; private set; }
    
    public void UpdateCnhImage(string url) => CnhImageUrl = url;
}