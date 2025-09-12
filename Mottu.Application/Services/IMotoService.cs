using DevFreela.Application.DTOs;
using Mottu.Application.DTOs;

namespace Mottu.Application.Services;

public interface IMotoService
{
    Task<ResultViewModel<Guid>> CreateAsync(CreateMotoDto dto);
    Task<ResultViewModel<List<MotoDto>>> ListAsync(string? placa);
    Task<ResultViewModel<MotoDto>> GetByIdAsync(Guid id);
    Task<ResultViewModel<string>> UpdatePlacaAsync(Guid id, UpdatePlacaDto dto);
    Task<ResultViewModel> DeleteAsync(Guid id);
    Task<ResultViewModel> InactivateAsync(Guid id);
}