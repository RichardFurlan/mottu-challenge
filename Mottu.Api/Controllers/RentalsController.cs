using Microsoft.AspNetCore.Mvc;
using Mottu.Application.DTOs;
using Mottu.Application.Services;

namespace Mottu.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRentalDto dto)
    {
        var result = await _rentalService.CreateAsync(dto);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _rentalService.GetByIdAsync(id);
        if (!result.IsSuccess) 
            return NotFound(result);
        return Ok(result);
    }

    [HttpPost("{id:guid}/return")]
    public async Task<IActionResult> Return(Guid id, [FromBody] ReturnRentalDto dto)
    {
        var result = await _rentalService.ReturnRentalAsync(id, dto);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return Ok(result); 
    }
}