using Mottu.Domain.Entities;

namespace Mottu.Application.DTOs;

public record CreateRentalDto(
    Guid MotoId,
    Guid EntregadorId,
    int PlanDays
)
{
    public Rental ToEntity(DateTime startDate, DateTime expectedEndDate, decimal dailyPrice) => new Rental(MotoId, EntregadorId, startDate, expectedEndDate, PlanDays, dailyPrice);
}