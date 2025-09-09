using DevFreela.Application.DTOs;
using Mottu.Application.Contracts.Messaging;
using Mottu.Application.DTOs;
using Mottu.Domain.Repositories;

namespace Mottu.Application.Services;

public class MotoService
{
    private readonly IMotoRepository _motoRepository;
    private readonly IMotoPublisher _publisher;

    public MotoService(IMotoRepository motoRepository, IMotoPublisher publisher)
    {
        _motoRepository = motoRepository;
        _publisher = publisher;
    }

    public async Task<ResultViewModel<Guid>> CriarMotoAsync(CreateMotoDto dto)
    {
        if (await _motoRepository.ExistsByPlacaAsync(dto.Placa))
            return ResultViewModel<Guid>.Error("Placa já cadastrada");

        var moto = dto.ToEntity();

        await _motoRepository.AddAsync(moto);
        
        return ResultViewModel<Guid>.Success(moto.Id);
    }
}