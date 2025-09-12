using Microsoft.AspNetCore.Mvc;
using Mottu.Application.DTOs;
using Mottu.Application.Services;

namespace Mottu.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class EntregadoresController : ControllerBase
{
    private readonly IEntregadorService _entregadorService;

    public EntregadoresController(IEntregadorService entregadorService)
    {
        _entregadorService = entregadorService;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEntregadorDto dto)
    {
        var result = await _entregadorService.CreateAsync(dto);
        if (!result.IsSuccess) return BadRequest(result);
        var location = $"/entregadores/{result.Data}";
        return Created(location, new { id = result.Data });
    }
    
    [HttpPost("{id:guid}/cnh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadCnh(Guid id,[FromForm] UploadCnhRequest request)
    {
        var result = await _entregadorService.UploadCnhAsync(id, request.File);
        if (!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
}