using DevFreela.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Mottu.Application.Contracts.Storage;
using Mottu.Application.DTOs;
using Mottu.Domain.Repositories;

namespace Mottu.Application.Services;

public class EntregadorService : IEntregadorService
{
    private readonly IEntregadorRepository _entregadorRepository;
    private readonly IStorageService _storageService;

    public EntregadorService(IEntregadorRepository entregadorRepository, IStorageService storageService)
    {
        _entregadorRepository = entregadorRepository;
        _storageService = storageService;
    }

    #region CreateAsync
    public async Task<ResultViewModel<Guid>> CreateAsync(CreateEntregadorDto dto)
    {
        if (await _entregadorRepository.ExistsByCnpjAsync(dto.Cnpj)) return ResultViewModel<Guid>.Error("CNPJ já cadastrado");
        if (await _entregadorRepository.ExistsByCnhNumberAsync(dto.CnhNumber)) return ResultViewModel<Guid>.Error("Número da CNH já cadastrado");
        if (!IsValidCnhType(dto.CnhType)) return ResultViewModel<Guid>.Error("Tipo de CNH inválido");
        
        var entregador = dto.ToEntity();
        await _entregadorRepository.AddAsync(entregador);
        
        return ResultViewModel<Guid>.Success(entregador.Id);
    }
    #endregion

    #region UploadCnhAsync
    public async Task<ResultViewModel<string>> UploadCnhAsync(Guid entregadorId, IFormFile file)
    {
        var ent = await _entregadorRepository.GetByIdAsync(entregadorId);
        if (ent is null) 
            return ResultViewModel<string>.Error("Entregador não encontrado");
        
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext != ".png" && ext != ".bmp") return ResultViewModel<string>.Error("Formato de arquivo inválido (aceito: png, bmp)");

        var key = $"cnh/{entregadorId}{ext}";
        var url = await _storageService.SaveFileAsync(file.OpenReadStream(), key, file.ContentType);
        
        ent.UpdateCnhImage(url);
        await _entregadorRepository.SaveAsync();
        
        return ResultViewModel<string>.Success(ent?.CnhImageUrl);
    }
    #endregion

    
    private bool IsValidCnhType(string t) => new[] { "A", "B", "A+B" }.Contains(t);
}