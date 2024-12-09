using System.Security.Claims;
using System.Text;
using AutoMapper;
using GostStorage.Attributes;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using GostStorage.Repositories;
using GostStorage.Repositories.Interfaces;
using GostStorage.Services;
using GostStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(
    IDocsService docsService,
    IReferencesService referencesService,
    IFieldsService fieldsService,
    IDocStatisticsService docStatisticsService,
    IUsersRepository usersRepository,
    ISearchRepository searchRepository,
    IMapper mapper)
    : ControllerBase
{
    [Authorize(Roles = "Heisenberg,Admin")]
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

        await docStatisticsService.AddAsync(new DocStatisticEntity
        {
            OrgBranch = user!.OrgBranch,
            Action = ActionType.Create,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return Ok(docId);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
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

    [Authorize(Roles = "Heisenberg,Admin")]
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

        await docStatisticsService.AddAsync(new DocStatisticEntity
        {
            OrgBranch = user!.OrgBranch,
            Action = ActionType.Update,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return result;
    }

    [Authorize(Roles = "Heisenberg,Admin")]
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

        await docStatisticsService.AddAsync(new DocStatisticEntity
        {
            OrgBranch = user!.OrgBranch,
            Action = ActionType.Update,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return result;
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPut("change-status")]
    public async Task<IActionResult> ChangeStatus(ChangeStatusRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await docStatisticsService.AddAsync(new DocStatisticEntity
        {
            OrgBranch = user!.OrgBranch,
            Action = ActionType.Update,
            DocId = model.Id,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return await docsService.ChangeStatusAsync(model.Id, model.Status);
    }

    [Authorize]
    [HttpGet("{docId}")]
    public async Task<ActionResult<DocumentWithFieldsModel?>> GetDocument(long docId)
    {
        var document = await docsService.GetDocumentAsync(docId);
        return document is null ? NotFound() : new OkObjectResult(document);
    }

    [NoCache]
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<List<DocumentWithFieldsModel>>> GetDocuments(
        [FromQuery] SearchParametersModel parameters, [FromQuery] int limit = 10, [FromQuery] int lastId = 0)
    {
        return Ok(await docsService.GetDocumentsAsync(parameters, null, limit, lastId));
    }

    [NoCache]
    [Authorize]
    [HttpGet("all-valid")]
    public async Task<ActionResult<List<DocumentWithFieldsModel>>> GetValidDocuments(
        [FromQuery] SearchParametersModel parameters, [FromQuery] int limit = 10, [FromQuery] int lastId = 0)
    {
        return Ok(await docsService.GetDocumentsAsync(parameters, true, limit, lastId));
    }

    [NoCache]
    [Authorize]
    [HttpGet("all-canceled")]
    public async Task<ActionResult<List<DocumentWithFieldsModel>>> GetCanceledDocuments(
        [FromQuery] SearchParametersModel parameters, [FromQuery] int limit = 10, [FromQuery] int lastId = 0)
    {
        return Ok(await docsService.GetDocumentsAsync(parameters, false, limit, lastId));
    }

    [NoCache]
    [Authorize]
    [HttpGet("all-count")]
    public async Task<ActionResult<int>> GetDocumentsCount(
        [FromQuery] SearchParametersModel parameters)
    {
        return Ok(await docsService.GetDocumentsCountAsync(parameters, null));
    }

    [NoCache]
    [Authorize]
    [HttpGet("all-valid-count")]
    public async Task<ActionResult<int>> GetValidDocumentsCount(
        [FromQuery] SearchParametersModel parameters)
    {
        return Ok(await docsService.GetDocumentsCountAsync(parameters, true));
    }

    [NoCache]
    [Authorize]
    [HttpGet("all-canceled-count")]
    public async Task<ActionResult<int>> GetCanceledDocumentsCount(
        [FromQuery] SearchParametersModel parameters)
    {
        return Ok(await docsService.GetDocumentsCountAsync(parameters, false));
    }

    [NoCache]
    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult> SearchValidAsync([FromQuery] FtsSearchQuery parameters)
    {
        return new OkObjectResult(await docsService.SearchAsync(parameters));
    }

    [NoCache]
    [Authorize]
    [HttpGet("search-all")]
    public async Task<ActionResult> SearchValidAsync([FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        return new OkObjectResult(await docsService.SearchAllAsync(limit, offset));
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("index-all")]
    public async Task<ActionResult> IndexAllAsync()
    {
        await docsService.IndexAllDocumentsAsync();
        return Ok();
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("{docId}/upload-file")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFileForDocumentAsync([FromForm] UploadFileModel file, long docId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model is not valid");
        }

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId).ConfigureAwait(false);

        await docStatisticsService
            .AddAsync(new DocStatisticEntity
                {
                    OrgBranch = user!.OrgBranch,
                    Action = ActionType.Update,
                    DocId = docId,
                    Date = DateTime.UtcNow,
                    UserId = userId
                })
            .ConfigureAwait(false);

        await docsService.UploadFileForDocumentAsync(file, docId);

        var fileContent = new MemoryStream();
        await file.File.CopyToAsync(fileContent).ConfigureAwait(false);
        var document = await docsService.GetDocumentAsync(docId);

        var indexModel = new FtsIndexModel
        {
            Document = SearchHelper.SplitFieldsToIndexDocument(document.DocId, document.Primary, document.Actual),
            Text = Encoding.UTF8.GetString(fileContent.ToArray()),
        };
        
        await searchRepository.IndexDocumentAsync(indexModel).ConfigureAwait(false);
        return Ok();
    }
}