using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/gosts")]
public class GostsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IGostsService _gostsService;
    
    public GostsController(IGostsService gostsService, IMapper mapper)
    {
        _mapper = mapper;
        _gostsService = gostsService;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddNewGost(AddNewGostDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var newField = _mapper.Map<FieldEntity>(dto);

        var id = _gostsService.AddNewGost(newField);
        
        return Ok(id);
    }
}