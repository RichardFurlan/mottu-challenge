using DevFreela.Application.DTOs;
using Mottu.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Mottu.Application.Services;

public interface IEntregadorService
{
    Task<ResultViewModel<Guid>> CreateAsync(CreateEntregadorDto dto);
    Task<ResultViewModel<string>> UploadCnhAsync(Guid entregadorId, IFormFile file);
}