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
    
    /// <summary>
    /// Add new doc
    /// </summary>
    /// <returns>Id of new doc</returns>
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
    
    /// <summary>
    /// Delete doc by id
    /// </summary>
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
    
    /// <summary>
    /// Update primary info of doc
    /// </summary>
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
    
    /// <summary>
    /// Update actual field in document
    /// </summary>
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

    /// <summary>
    /// Change status of doc
    /// </summary>
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

    /// <summary>
    /// Get doc by id with its references
    /// </summary>
    /// <returns>Doc with actual and primary fileds and references</returns>
    [HttpGet("{docId}")]
    public async Task<ActionResult<GetDocumentResponseModel>> GetDocument(long docId)
    {
        await _docStatisticsService.UpdateViewsAsync(docId);
        
        return await _docsService.GetDocument(docId);
    }
    
    /// <summary>
    /// Get all documents without references
    /// </summary>
    /// <returns>List of any status document without references</returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<GetDocumentResponseModel>>> GetAllDocuments([FromQuery] SearchParametersModel parameters)
    {
        return Ok(await _docsService.GetAllDocuments(parameters));
    }
    
    /// <summary>
    /// Get only valid documents
    /// </summary>
    /// <returns>List of valid documents without references</returns>
    [HttpGet("all-valid")]
    public async Task<ActionResult<List<GetDocumentResponseModel>>> GetValidDocuments([FromQuery] SearchParametersModel parameters)
    {
        return Ok(await _docsService.GetAllDocuments(parameters, true));
    }
    
    /// <summary>
    /// Get only not valid documents
    /// </summary>
    /// <returns>List of replaced or canceled documents without references</returns>
    [HttpGet("all-canceled")]
    public async Task<ActionResult<List<GetDocumentResponseModel>>> GetCanceledDocuments([FromQuery] SearchParametersModel parameters)
    {
        return Ok(await _docsService.GetAllDocuments(parameters, false));
    }
}