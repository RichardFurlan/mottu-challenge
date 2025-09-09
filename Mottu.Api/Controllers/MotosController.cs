using Microsoft.AspNetCore.Mvc;
using Mottu.Application.DTOs;
using Mottu.Application.Services;
using Mottu.Domain.Repositories;

namespace Mottu.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MotosController : ControllerBase
{
    private readonly IMotoService _motoService;

    public MotosController(IMotoService motoService)
    {
        _motoService = motoService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] string? placa)
    {
        var result = await _motoService.ListarMotoAsync(placa);
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _motoService.BuscarMotoAsync(id);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMotoDto dto)
    {

        var result = await _motoService.CriarMotoAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }
    
    
    [HttpPut("{id:guid}/placa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePlaca(Guid id, [FromBody] UpdatePlacaDto dto)
    {
        var result = await _motoService.AtualizarPlacaAsync(id, dto);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
    
    [HttpPut("{id:guid}/inativar")]
    public async Task<IActionResult> Inativar(Guid id)
    {
        var result = await _motoService.InativarMotoAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Message);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _motoService.DeletarMotoAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Message);
        return NoContent();
    }
}