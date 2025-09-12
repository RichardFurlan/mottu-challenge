using DevFreela.Application.DTOs;
using Mottu.Application.DTOs;
using Mottu.Domain.Entities;

namespace Mottu.Application.Services;

public interface IRentalService
{
    Task<ResultViewModel<Guid>> CreateAsync(CreateRentalDto dto);
    Task<ResultViewModel<RentalDto>> GetByIdAsync(Guid id);
    Task<ResultViewModel<decimal>> ReturnRentalAsync(Guid rentalId, ReturnRentalDto dto);
}