using DevFreela.Application.DTOs;
using Mottu.Application.Contracts.Storage;
using Mottu.Application.DTOs;
using Mottu.Domain.Repositories;

namespace Mottu.Application.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IEntregadorRepository _entregadorRepository;
    private readonly IMotoRepository _motoRepository;

    public RentalService(IRentalRepository rentalRepository, IEntregadorRepository entregadorRepository, IMotoRepository motoRepository)
    {
        _rentalRepository = rentalRepository;
        _entregadorRepository = entregadorRepository;
        _motoRepository = motoRepository;
    }

    public async Task<ResultViewModel<Guid>> CreateAsync(CreateRentalDto dto)
    {
        var moto = await _motoRepository.GetMotoByIdAsync(dto.MotoId);
        if (moto is null)
            return ResultViewModel<Guid>.Error("Moto não encontrada");
        if (!moto.IsActive || moto.IsDeleted) 
            return ResultViewModel<Guid>.Error("Moto não disponível para locação");
        
        if (await _rentalRepository.ExistsActiveRentalByMotoIdAsync(dto.MotoId))
            return ResultViewModel<Guid>.Error("Moto já alugada");

        var entregador = await _entregadorRepository.GetByIdAsync(dto.EntregadorId);
        if (entregador is null)
            return ResultViewModel<Guid>.Error("Entregador não encontrado");
        if(!IsCnhAllowed(entregador.CnhType))
            return ResultViewModel<Guid>.Error("Entregador não habilitado para locação (CNH inválida)");

        var (days, pricePerDay) = PlanInfo(dto.PlanDays);
        if (days == 0) return ResultViewModel<Guid>.Error("Plano inválido");
        
        var startDate = DateTime.UtcNow.Date.AddDays(1);
        var expected = startDate.AddDays(days - 1);
        
        var rental = dto.ToEntity(startDate, expected, pricePerDay);
        
        await _rentalRepository.AddAsync(rental);

        return ResultViewModel<Guid>.Success(rental.Id);
    }

    public async Task<ResultViewModel<RentalDto>> GetByIdAsync(Guid id)
    {
        var rental = await _rentalRepository.GetByIdAsync(id);
        if (rental == null)
            return ResultViewModel<RentalDto>.Error("Locação não encontrada");

        var dto = RentalDto.FromEntity(rental);
        return ResultViewModel<RentalDto>.Success(dto);
    }

    public async Task<ResultViewModel<decimal>> ReturnRentalAsync(Guid rentalId, ReturnRentalDto dto)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental is null)
            return ResultViewModel<decimal>.Error("Locação não encontrada");

        var actual = dto.ActualReturnDate.Date;
        
        var total = CalculateReturnAmount(
            rental.StartDate, rental.ExpectedEndDate, actual, rental.PlanDays, rental.DailyPrice);
        return ResultViewModel<decimal>.Success(total);
    }
    
    private bool IsCnhAllowed(string cnh) => cnh.Contains("A");
    
    private (int days, decimal pricePerDay) PlanInfo(int planDays) => planDays switch
    {
        7 => (7, 30m),
        15 => (15, 28m),
        30 => (30, 22m),
        45 => (45, 20m),
        50 => (50, 18m),
        _ => (0, 0m)
    };
    
    private decimal CalculateReturnAmount(DateTime start, DateTime expected, DateTime actual, int planDays, decimal dailyPrice)
    {
        var daysUsed = (int)(actual.Date - start.Date).TotalDays + 1;
        if (daysUsed < 1) daysUsed = 1;

        if (actual <= expected)
        {
            var daysNotUsed = planDays - daysUsed;
            if (daysNotUsed < 0) daysNotUsed = 0;
            var baseValue = daysUsed * dailyPrice;
            var penaltyRate = planDays == 7 ? 0.20m : planDays == 15 ? 0.40m : 0m;
            var penalty = daysNotUsed * dailyPrice * penaltyRate;
            return baseValue + penalty;
        }
        else
        {
            var baseValue = planDays * dailyPrice;
            var extraDays = (int)(actual.Date - expected.Date).TotalDays;
            var extra = extraDays * 50m;
            return baseValue + extra;
        }
    }
}