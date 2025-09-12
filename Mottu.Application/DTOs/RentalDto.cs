using Mottu.Domain.Entities;

namespace Mottu.Application.DTOs;

public record RentalDto(
    Guid Id,
    Guid MotoId,
    Guid EntregadorId,
    DateTime StartDate,
    DateTime ExpectedEndDate,
    DateTime? ActualReturnDate,
    int PlanDays,
    decimal DailyPrice
)
{
    public static RentalDto FromEntity(Rental entity) => 
        new RentalDto(
            entity.Id, 
            entity.MotoId, 
            entity.EntregadorId, 
            entity.StartDate, 
            entity.ExpectedEndDate, 
            entity.ActualReturnDate, 
            entity.PlanDays, 
            entity.DailyPrice
        );
}