using DevFreela.Application.DTOs;
using Mottu.Application.Contracts.Messaging;
using Mottu.Application.DTOs;
using Mottu.Application.Events;
using Mottu.Domain.Entities;
using Mottu.Domain.Repositories;

namespace Mottu.Application.Services;

public class MotoService : IMotoService
{
    private readonly IMotoRepository _motoRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IMotoPublisher _publisher;

    public MotoService(IMotoRepository motoRepository, IMotoPublisher publisher, IRentalRepository rentalRepository)
    {
        _motoRepository = motoRepository;
        _publisher = publisher;
        _rentalRepository = rentalRepository;
    }

    #region CreateAsync

    public async Task<ResultViewModel<Guid>> CreateAsync(CreateMotoDto dto)
    {
        if (await _motoRepository.ExistsByPlacaAsync(dto.Placa))
            return ResultViewModel<Guid>.Error("Placa já cadastrada");

        var moto = dto.ToEntity();

        await _motoRepository.AddAsync(moto);
        await _publisher.PublishWithRetryAsync(new MotoCreatedIntegrationEvent(
            moto.Id, moto.Ano, moto.Modelo, moto.Placa
        ));

        return ResultViewModel<Guid>.Success(moto.Id);
    }

    #endregion

    #region ListAsync

    public async Task<ResultViewModel<List<MotoDto>>> ListAsync(string? placa)
    {
        var motos = await _motoRepository.GetMotosAsync(placa);

        var motosDto = motos
            .Select(m => new MotoDto(m.Id, m.Ano, m.Modelo, m.Placa, m.CreatedAt, m.IsActive, m.IsDeleted))
            .ToList();

        return ResultViewModel<List<MotoDto>>.Success(motosDto);
    }

    #endregion

    #region GetByIdAsync

    public async Task<ResultViewModel<MotoDto>> GetByIdAsync(Guid id)
    {
        var moto = await _motoRepository.GetMotoByIdAsync(id);

        if (moto is null)
            return ResultViewModel<MotoDto>.Error("Moto não encontrada");

        var motoDto = MotoDto.FromEntity(moto);
        return ResultViewModel<MotoDto>.Success(motoDto);
    }

    #endregion

    #region UpdatePlacaAsync

    public async Task<ResultViewModel<string>> UpdatePlacaAsync(Guid id, UpdatePlacaDto dto)
    {
        if (await _motoRepository.ExistsByPlacaAsync(dto.Placa))
            return ResultViewModel<string>.Error("Já existe uma moto cadastrada com essa placa");

        var moto = await _motoRepository.GetMotoByIdAsync(id);
        if (moto is null)
            return ResultViewModel<string>.Error("Moto não encontrada");

        moto.UpdatePlaca(dto.Placa);
        await _motoRepository.SaveAsync();

        return ResultViewModel<string>.Success("Placa modificada com sucesso");
    }

    #endregion

    #region DeleteAsync

    public async Task<ResultViewModel> DeleteAsync(Guid id)
    {
        if (await _rentalRepository.ExistsActiveRentalByMotoIdAsync(id))
            return ResultViewModel.Error("Não é possivel remover, pois a moto possui locações");
        
        var moto = await _motoRepository.GetMotoByIdAsync(id);
        if (moto is null)
            return ResultViewModel<string>.Error("Moto não encontrada");
        if (moto.IsActive)
            return ResultViewModel<string>.Error("Não é possivel remover motos ativas, desative a moto antes de remover");
        
        moto.Delete();
        await _motoRepository.SaveAsync();
        
        return ResultViewModel.Success();
    }

    #endregion

    #region InactivateAsync
    public async Task<ResultViewModel> InactivateAsync(Guid id)
    {
        if (await _rentalRepository.ExistsActiveRentalByMotoIdAsync(id))
            return ResultViewModel.Error("Não é possivel inativar, pois a moto possui locações");
        
        var moto = await _motoRepository.GetMotoByIdAsync(id);
        if (moto is null)
            return ResultViewModel<string>.Error("Moto não encontrada");
        
        moto.Inativar();
        await _motoRepository.SaveAsync();
        
        return ResultViewModel.Success();
    }
    #endregion
}