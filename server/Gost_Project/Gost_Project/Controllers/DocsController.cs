using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Helpers;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(IDocsService docsService, IMapper mapper, IReferencesService referencesService) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IDocsService _docsService = docsService;
    private readonly IReferencesService _referencesService = referencesService;
    
    [HttpPost("add")]
    public async Task<IActionResult> AddNewDoc([FromBody] AddNewDocDtoModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var newField = _mapper.Map<FieldEntity>(dto);
        var id = _docsService.AddNewDoc(newField);
        _referencesService.AddReferences(dto.ReferencesId, id);
        
        return Ok(id);
    }
}