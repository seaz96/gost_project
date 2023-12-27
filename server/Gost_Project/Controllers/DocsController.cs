using AutoMapper;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(IDocsService docsService, IMapper mapper,
    IReferencesService referencesService, IFieldsService fieldsService, IDocStatisticsService docStatisticsService) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IDocsService _docsService = docsService;
    private readonly IReferencesService _referencesService = referencesService;
    private readonly IFieldsService _fieldsService = fieldsService;
    private readonly IDocStatisticsService _docStatisticsService = docStatisticsService;
    
    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<IActionResult> AddNewDoc([FromBody] AddNewDocDtoModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var newField = _mapper.Map<FieldEntity>(dto);
        var docId = await _docsService.AddNewDocAsync(newField);
        await _referencesService.AddReferencesAsync(dto.ReferencesId, docId);
        await _docStatisticsService.AddNewDocStatsAsync(docId);
        
        return Ok(docId);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{docId}")]
    public async Task<IActionResult> DeleteDoc(long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }
        
        var result = await _docsService.DeleteDocAsync(docId);
        await _referencesService.DeleteReferencesByIdAsync(docId);
        await _docStatisticsService.DeleteAsync(docId);
        
        return result;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("update/{docId}")]
    public async Task<IActionResult> Update([FromBody] UpdateFieldDtoModel dto, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var updatedField = _mapper.Map<FieldEntity>(dto);
        var result = await _fieldsService.UpdateAsync(updatedField, docId);
        await _referencesService.UpdateReferencesAsync(dto.ReferencesId, docId);
        await _docStatisticsService.UpdateChangedAsync(docId);
        
        return result;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("actualize/{docId}")]
    public async Task<IActionResult> Actualize([FromBody] UpdateFieldDtoModel dto, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var updatedField = _mapper.Map<FieldEntity>(dto);
        var result = await _fieldsService.ActualizeAsync(updatedField, docId);
        await _referencesService.UpdateReferencesAsync(dto.ReferencesId, docId);
        await _docStatisticsService.UpdateChangedAsync(docId);

        return result;
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("change-status")]
    public async Task<IActionResult> ChangeStatus(ChangeStatusRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }
        
        await _docStatisticsService.UpdateChangedAsync(model.Id);
        
        return await _docsService.ChangeStatusAsync(model.Id, model.Status);
    }

    [HttpGet("{docId}")]
    public async Task<ActionResult<GetDocumentResponseModel>> GetDocument(long docId)
    {
        await _docStatisticsService.UpdateViewsAsync(docId);
        
        return await _docsService.GetDocument(docId);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<GetDocumentResponseModel>>> GetAllDocuments()
    {
        return Ok(await _docsService.GetAllDocuments());
    }
}