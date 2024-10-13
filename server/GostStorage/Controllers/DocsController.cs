using System.Security.Claims;
using AutoMapper;
using GostStorage.Attributes;
using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using GostStorage.Repositories.Interfaces;
using GostStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(
    IDocsService docsService,
    IMapper mapper,
    IReferencesService referencesService,
    IFieldsService fieldsService,
    IDocStatisticsService docStatisticsService,
    IUsersRepository usersRepository,
    ISearchRepository searchRepository) : ControllerBase
{
    /// <summary>
    /// Add new doc
    /// </summary>
    /// <returns>Id of new doc</returns>
    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpPost("add")]
    public async Task<IActionResult> AddNewDoc([FromBody] AddNewDocDtoModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var newField = mapper.Map<FieldEntity>(dto);
        
        var docId = await docsService.AddNewDocAsync(newField);
        await referencesService.AddReferencesAsync(dto.References, docId);

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);
        
        await docStatisticsService.AddAsync(new DocStatisticEntity { OrgBranch = user!.OrgBranch, Action = ActionType.Create, DocId = docId, Date = DateTime.UtcNow, UserId = userId});

        return Ok(docId);
    }

    /// <summary>
    /// Delete doc by id
    /// </summary>
    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpDelete("delete/{docId}")]
    public async Task<IActionResult> DeleteDoc(long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var result = await docsService.DeleteDocAsync(docId);
        await referencesService.DeleteReferencesByIdAsync(docId);
        await docStatisticsService.DeleteAsync(docId);
        await searchRepository.DeleteDocumentAsync(docId);
        
        return result;
    }

    /// <summary>
    /// Update primary info of doc
    /// </summary>
    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpPut("update/{docId}")]
    public async Task<IActionResult> Update([FromBody] UpdateFieldDtoModel dto, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var updatedField = mapper.Map<FieldEntity>(dto);
        updatedField.DocId = docId;
        var result = await fieldsService.UpdateAsync(updatedField, docId);
        await referencesService.UpdateReferencesAsync(dto.References, docId);
        
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await docStatisticsService.AddAsync(new DocStatisticEntity { OrgBranch = user!.OrgBranch, Action = ActionType.Update, DocId = docId, Date = DateTime.UtcNow, UserId = userId});

        return result;
    }

    /// <summary>
    /// Update actual field in document
    /// </summary>
    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpPut("actualize/{docId}")]
    public async Task<IActionResult> Actualize([FromBody] UpdateFieldDtoModel dto, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var updatedField = mapper.Map<FieldEntity>(dto);
        updatedField.DocId = docId;
        var result = await fieldsService.ActualizeAsync(updatedField, docId);
        await referencesService.UpdateReferencesAsync(dto.References, docId);
        
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await docStatisticsService.AddAsync(new DocStatisticEntity {OrgBranch = user!.OrgBranch, Action = ActionType.Update, DocId = docId, Date = DateTime.UtcNow, UserId = userId});

        return result;
    }

    /// <summary>
    /// Change status of doc
    /// </summary>
    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpPut("change-status")]
    public async Task<IActionResult> ChangeStatus(ChangeStatusRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await docStatisticsService.AddAsync(new DocStatisticEntity { OrgBranch = user!.OrgBranch, Action = ActionType.Update, DocId = model.Id, Date = DateTime.UtcNow, UserId = userId});
        
        return await docsService.ChangeStatusAsync(model.Id, model.Status);
    }

    /// <summary>
    /// Get doc by id with its references
    /// </summary>
    /// <returns>Doc with actual and primary fields and references</returns>
    [HttpGet("{docId}")]
    public async Task<ActionResult<DocumentWithFieldsModel>> GetDocument(long docId)
    {
        return await docsService.GetDocumentAsync(docId);
    }

    /// <summary>
    /// Get all documents without references
    /// </summary>
    /// <returns>List of any status document without references</returns>
    [NoCache]
    [HttpGet("all")]
    public async Task<ActionResult<List<DocumentWithFieldsModel>>> GetDocuments(
        [FromQuery] SearchParametersModel parameters, [FromQuery] int limit = 10, [FromQuery] int lastId = 0)
    {
        return Ok(await docsService.GetDocumentsAsync(parameters, null, limit, lastId));
    }

    /// <summary>
    /// Get only valid documents
    /// </summary>
    /// <returns>List of valid documents without references</returns>
    [NoCache]
    [HttpGet("all-valid")]
    public async Task<ActionResult<List<DocumentWithFieldsModel>>> GetValidDocuments(
        [FromQuery] SearchParametersModel parameters, [FromQuery] int limit = 10, [FromQuery] int lastId = 0)
    {
        return Ok(await docsService.GetDocumentsAsync(parameters, true, limit, lastId));    
    }

    /// <summary>
    /// Get only not valid documents
    /// </summary>
    /// <returns>List of replaced or canceled documents without references</returns>
    [NoCache]
    [HttpGet("all-canceled")]
    public async Task<ActionResult<List<DocumentWithFieldsModel>>> GetCanceledDocuments(
        [FromQuery] SearchParametersModel parameters, [FromQuery] int limit = 10, [FromQuery] int lastId = 0)
    {
        return Ok(await docsService.GetDocumentsAsync(parameters, false, limit, lastId));
    }
    
    /// <summary>
    /// Get count of all documents without references
    /// </summary>
    /// <returns>List of any status document without references</returns>
    [NoCache]
    [HttpGet("all-count")]
    public async Task<ActionResult<int>> GetDocumentsCount(
        [FromQuery] SearchParametersModel parameters)
    {
        return Ok(await docsService.GetDocumentsCountAsync(parameters, null));
    }

    /// <summary>
    /// Count count of only valid documents
    /// </summary>
    /// <returns>List of valid documents without references</returns>
    [NoCache]
    [HttpGet("all-valid-count")]
    public async Task<ActionResult<int>> GetValidDocumentsCount(
        [FromQuery] SearchParametersModel parameters)
    {
        return Ok(await docsService.GetDocumentsCountAsync(parameters, true));    
    }

    /// <summary>
    /// Get count of only not valid documents
    /// </summary>
    /// <returns>List of replaced or canceled documents without references</returns>
    [NoCache]
    [HttpGet("all-canceled-count")]
    public async Task<ActionResult<int>> GetCanceledDocumentsCount(
        [FromQuery] SearchParametersModel parameters)
    {
        return Ok(await docsService.GetDocumentsCountAsync(parameters, false));
    }

    /// <summary>
    /// Get all docs with general info only
    /// </summary>
    [NoCache]
    [HttpGet("all-general-info")]
    public async Task<ActionResult> GetDocsWithGeneralInfo()
    {
        return Ok(await docsService.GetDocsWithGeneralInfoAsync());
    }
    
    [HttpGet("search-valid")]
    public async Task<ActionResult> SearchValidAsync([FromQuery] SearchParametersModel parameters,
        [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        return new OkObjectResult(await docsService.SearchValidAsync(parameters, limit, offset));
    }
    
    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpPost("index-all")]
    public async Task<ActionResult> IndexAllAsync()
    {
        _ = docsService.IndexAllDocumentsAsync();
        return Ok();
    }


    [Authorize(Roles = "Admin,Heisenberg")]
    [HttpPost("{docId}/upload-file")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFileForDocumentAsync([FromForm] UploadFileModel file, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await docStatisticsService.AddAsync(new DocStatisticEntity
        {
            OrgBranch = user!.OrgBranch, Action = ActionType.Update, DocId = docId, Date = DateTime.UtcNow,
            UserId = userId
        });

        await docsService.IndexDocumentDataAsync(file.File, docId);
        
        return Ok(await docsService.UploadFileForDocumentAsync(file, docId));
    }
}