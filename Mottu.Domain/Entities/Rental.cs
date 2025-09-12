namespace Mottu.Domain.Entities;

public class Rental : EntityBase
{
    public Rental(Guid motoId, Guid entregadorId, DateTime startDate, DateTime expectedEndDate, int planDays, decimal dailyPrice)
    {
        MotoId = motoId;
        EntregadorId = entregadorId;
        StartDate = startDate;
        ExpectedEndDate = expectedEndDate;
        PlanDays = planDays;
        DailyPrice = dailyPrice;
    }
    public Guid MotoId { get; private set; }
    public Guid EntregadorId { get; private set; }
    
    public Moto? Moto { get; private set; }
    public Entregador? Entregador { get; private set; }
    
    public DateTime StartDate { get; private set; }
    public DateTime ExpectedEndDate { get; private set; }
    public DateTime? ActualReturnDate { get; private set; }

    public int PlanDays { get; private set; }
    public decimal DailyPrice { get; private set; }

    public void SetActualReturn(DateTime dt) => ActualReturnDate = dt;
}