using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(IDocsService docsService, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;

    private readonly IDocsService _docsService = docsService;

    [HttpPost("add")]
    public async Task<IActionResult> AddNewDoc(AddNewDocDtoModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var newField = _mapper.Map<FieldEntity>(dto);

        var id = _docsService.AddNewDoc(newField);

        return Ok(id);
    }
}