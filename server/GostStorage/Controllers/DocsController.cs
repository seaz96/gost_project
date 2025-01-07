using System.Security.Claims;
using AutoMapper;
using GostStorage.Attributes;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using GostStorage.Repositories.Abstract;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/docs")]
public class DocsController(
    IDocumentsService documentsService,
    IReferencesService referencesService,
    IFieldsService fieldsService,
    IUserActionsService userActionsService,
    IUsersRepository usersRepository,
    ISearchRepository searchRepository,
    IFileProcessor fileProcessor,
    IMapper mapper)
    : ControllerBase
{
    [Authorize(Roles = "Heisenberg,Admin")]
    [Consumes("application/json")]
    [HttpPost("add")]
    public async Task<ActionResult<long>> AddDocument([FromBody] AddDocumentRequest dto)
    {
        var newField = mapper.Map<PrimaryField>(dto);

        var docId = await documentsService.AddDocumentAsync(newField, DocumentStatus.Valid);
        await referencesService.AddReferencesAsync(dto.References, docId);

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await userActionsService.AddAsync(new UserAction
        {
            OrgBranch = user!.OrgBranch,
            Type = ActionType.Create,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return Ok(docId);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [Consumes("multipart/form-data")]
    [HttpPost("add/with-file")]
    public async Task<ActionResult<long>> AddDocument([FromForm] AddDocumentWithFileRequest request)
    {
        var documentId = await AddDocument(request as AddDocumentRequest);
        await UploadFileForDocumentAsync(new UploadFileRequest { File = request.File }, documentId.Value);

        return new OkObjectResult(documentId);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpDelete("delete/{docId}")]
    public async Task<IActionResult> Delete(long docId)
    {
        await referencesService.DeleteReferencesByIdAsync(docId);
        await userActionsService.DeleteAsync(docId);
        await searchRepository.DeleteDocumentAsync(docId);
        var result = await documentsService.DeleteDocumentAsync(docId);

        return result
            ? new OkObjectResult("Document deleted successfully.")
            : new NotFoundObjectResult($"Document with id {docId} not found.");
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPut("update/{docId}")]
    public async Task<IActionResult> Update([FromBody] UpdateDocumentRequest dto, long docId)
    {
        var updatedField = mapper.Map<Field>(dto);
        updatedField.DocId = docId;
        var result = await fieldsService.UpdateAsync(updatedField, docId);
        await referencesService.UpdateReferencesAsync(dto.References, docId);

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await userActionsService.AddAsync(new UserAction
        {
            OrgBranch = user!.OrgBranch,
            Type = ActionType.Update,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return result;
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPut("actualize/{docId}")]
    public async Task<IActionResult> Actualize([FromBody] UpdateDocumentRequest dto, long docId)
    {
        var updatedField = mapper.Map<Field>(dto);
        updatedField.DocId = docId;
        var result = await fieldsService.ActualizeAsync(updatedField, docId);
        await referencesService.UpdateReferencesAsync(dto.References, docId);

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await userActionsService.AddAsync(new UserAction
        {
            OrgBranch = user!.OrgBranch,
            Type = ActionType.Update,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return result;
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPut("change-status")]
    public async Task<IActionResult> ChangeStatus(ChangeStatusRequest model)
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await userActionsService.AddAsync(new UserAction
        {
            OrgBranch = user!.OrgBranch,
            Type = ActionType.Update,
            DocId = model.Id,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return await documentsService.ChangeStatusAsync(model.Id, model.Status);
    }

    [Authorize]
    [HttpGet("{docId}")]
    public async Task<ActionResult<FullDocument?>> GetDocument(long docId)
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await userActionsService.AddAsync(new UserAction
        {
            OrgBranch = user!.OrgBranch,
            Type = ActionType.View,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });
        
        var document = await documentsService.GetDocumentAsync(docId);
        return document is null ? NotFound() : new OkObjectResult(mapper.Map<FullDocumentResponse>(document));
    }

    [NoCache]
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<List<FullDocument>>> GetDocuments(
        [FromQuery] GetDocumentRequest parameters)
    {
        if (parameters.Limit < 0 || parameters.Offset < 0)
            return new BadRequestObjectResult("Limit or offset cannot be negative");
        
        return Ok(
            (await documentsService.GetDocumentsAsync(parameters))
                .Select(mapper.Map<FullDocumentResponse>)
                .ToList());
    }

    [NoCache]
    [Authorize]
    [HttpGet("all-count")]
    public async Task<ActionResult<int>> GetDocumentsCount(
        [FromQuery] GetDocumentRequest parameters)
    {
        if (parameters.Limit < 0 || parameters.Offset < 0)
            return new BadRequestObjectResult("Limit or offset cannot be negative");
        
        return Ok(await documentsService.GetDocumentsCountAsync(parameters));
    }

    [NoCache]
    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult> SearchAsync([FromQuery] SearchQuery parameters)
    {
        if (parameters.Limit < 0 || parameters.Offset < 0)
            return new BadRequestObjectResult("Limit or offset cannot be negative");
        
        return new OkObjectResult(await documentsService.SearchAsync(parameters));
    }
    
    [NoCache]
    [Authorize]
    [HttpGet("search-count")]
    public async Task<ActionResult> SearchCountAsync([FromQuery] SearchQuery parameters)
    {
        if (parameters.Limit < 0 || parameters.Offset < 0)
            return new BadRequestObjectResult("Limit or offset cannot be negative");
        
        return new OkObjectResult(await documentsService.SearchCountAsync(parameters));
    }
    
    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("index-all")]
    public async Task<ActionResult> IndexAllAsync()
    {
        await documentsService.IndexAllDocumentsAsync();
        return Ok();
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("{docId}/upload-file")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFileForDocumentAsync([FromForm] UploadFileRequest request, long docId)
    {
        var document = await documentsService.GetDocumentAsync(docId);
        if (document is null)
        {
            return NotFound($"Document with id {docId} not found");
        }

        var isUploadSuccesful = await documentsService.UploadFileForDocumentAsync(request.File, docId);

        if (!isUploadSuccesful)
        {
            return BadRequest("File is not uploaded. Check file.");
        }
        
        var indexModel = new SearchIndexModel
        {
            Document = SearchHelper.SplitFieldsToIndexDocument(document.DocId, document.Primary, document.Actual),
            Text = await fileProcessor.ExtractFileTextSafeAsync(request.File)
        };
        
        await searchRepository.IndexDocumentAsync(indexModel).ConfigureAwait(false);

        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId).ConfigureAwait(false);

        await userActionsService
            .AddAsync(new UserAction
            {
                OrgBranch = user!.OrgBranch,
                Type = ActionType.Update,
                DocId = docId,
                Date = DateTime.UtcNow,
                UserId = userId
            })
            .ConfigureAwait(false);
        
        return Ok();
    }
}