using DevFreela.Application.DTOs;
using Mottu.Application.DTOs;

namespace Mottu.Application.Services;

public interface IMotoService
{
    Task<ResultViewModel<Guid>> CriarMotoAsync(CreateMotoDto dto);
    Task<ResultViewModel<List<MotoDto>>> ListarMotoAsync(string? placa);
    Task<ResultViewModel<MotoDto>> BuscarMotoAsync(Guid id);
    Task<ResultViewModel<string>> AtualizarPlacaAsync(Guid id, UpdatePlacaDto dto);
    Task<ResultViewModel> DeletarMotoAsync(Guid id);
    Task<ResultViewModel> InativarMotoAsync(Guid id);
}