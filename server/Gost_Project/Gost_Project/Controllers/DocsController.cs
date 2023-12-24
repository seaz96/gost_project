using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Helpers;
using Gost_Project.Services.Abstract;
using Gost_Project.Services.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(IDocsService docsService, IMapper mapper,
    IReferencesService referencesService, IFieldsService fieldsService) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IDocsService _docsService = docsService;
    private readonly IReferencesService _referencesService = referencesService;
    private readonly IFieldsService _fieldsService = fieldsService;
    
    [HttpPost("add")]
    public async Task<IActionResult> AddNewDoc([FromBody] AddNewDocDtoModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var newField = _mapper.Map<FieldEntity>(dto);
        var docId = _docsService.AddNewDoc(newField);
        _referencesService.AddReferences(dto.ReferencesId, docId);
        
        return Ok(docId);
    }

    [HttpDelete("delete/{docId}")]
    public async Task<IActionResult> DeleteDoc(long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        docsService.DeleteDoc(docId);
        referencesService.DeleteReferencesById(docId);
        
        return Ok();
    }
    
    [HttpPut("update/{docId}")]
    public async Task<IActionResult> Update([FromBody] UpdateFieldDtoModel dto, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var updatedField = _mapper.Map<FieldEntity>(dto);
        _fieldsService.Update(updatedField, docId);
        _referencesService.UpdateReferences(dto.ReferencesId, docId);

        return Ok();
    }
    
    [HttpPut("actualize/{docId}")]
    public async Task<IActionResult> Actualize([FromBody] UpdateFieldDtoModel dto, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var updatedField = _mapper.Map<FieldEntity>(dto);
        _fieldsService.Update(updatedField, docId);
        _referencesService.UpdateReferences(dto.ReferencesId, docId);

        return Ok();
    }

    [HttpPut("change-status")]
    public async Task<IActionResult> ChangeStatus(ChangeStatusRequestModel model)
    {
        return _docsService.ChangeStatus(model.Id, model.Status);
    }
}