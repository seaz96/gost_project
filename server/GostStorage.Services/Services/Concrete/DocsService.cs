using Elastic.Clients.Elasticsearch;
using GostStorage.Domain.Entities;
using GostStorage.Domain.Models;
using GostStorage.Domain.Navigations;
using GostStorage.Domain.Repositories;
using GostStorage.Services.Helpers;
using GostStorage.Services.Models.Docs;
using GostStorage.Services.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Services.Concrete;

public class DocsService(IDocsRepository docsRepository, IFieldsRepository fieldsRepository,
    IReferencesRepository referencesRepository, IFieldsService fieldsService, IFilesRepository filesRepository,
    ISearchRepository searchRepository) : IDocsService
{
    private readonly IDocsRepository _docsRepository = docsRepository;
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    private readonly IFieldsService _fieldsService = fieldsService;
    private readonly IFilesRepository _filesRepository = filesRepository;
    private readonly ISearchRepository _searchRepository = searchRepository;
    
    
    public async Task<long> AddNewDocAsync(FieldEntity primaryField)
    {
        if (primaryField.Designation is not null)
        {
            primaryField.Designation = TextFormattingHelper.FormatDesignation(primaryField.Designation);
        }
        var doc = await _docsRepository.GetByDesignationAsync(primaryField.Designation);
        
        if (doc is not null)
        {
            await _fieldsService.UpdateAsync(primaryField, doc.Id);
            await ChangeStatusAsync(doc.Id, primaryField.Status);
            return doc.Id;
        }
        
        var actualField = new FieldEntity
        {
            LastEditTime = DateTime.UtcNow,
            Status = primaryField.Status
        };
        var primaryId = await _fieldsRepository.AddAsync(primaryField);
        var actualId = await _fieldsRepository.AddAsync(actualField);
        
        doc = new DocEntity { ActualFieldId = actualId, PrimaryFieldId = primaryId };
        
        var docId = await _docsRepository.AddAsync(doc);
        
        if (docId == 0)
        {
            await Task.Delay(1000);
        }
        
        primaryField.DocId = docId;
        actualField.DocId = docId; 
        
        await _fieldsRepository.UpdateAsync(primaryField);
        await _fieldsRepository.UpdateAsync(actualField);

        IndexAllDocumentsAsync();

        return docId;
    }

    public async Task<IActionResult> DeleteDocAsync(long id)
    {
        var doc = await _docsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        await _fieldsRepository.DeleteAsync(doc.PrimaryFieldId);
        await _fieldsRepository.DeleteAsync(doc.ActualFieldId);
        
        await _docsRepository.DeleteAsync(id);
        
        IndexAllDocumentsAsync();

        return new OkObjectResult("Document deleted successfully.");
    }

    public async Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status)
    {
        var doc = await _docsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        var primaryField = await _fieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        var actualField = await _fieldsRepository.GetByIdAsync(doc.ActualFieldId);
        
        if (primaryField is not null)
        {
            primaryField.Status = status;
            await _fieldsRepository.UpdateAsync(primaryField);
        }
        if (actualField is not null)
        {
            actualField.Status = status;
            await _fieldsRepository.UpdateAsync(actualField);
        }
        
        IndexAllDocumentsAsync();

        return new OkObjectResult("Status changed successfully.");
    }

    public async Task<ActionResult<DocumentWithFieldsModel>> GetDocumentAsync(long id)
    {
        var doc = await _docsRepository.GetByIdAsync(id);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();
        
        var references = (await _referencesRepository.GetAllAsync())
            .Where(reference => reference.ParentalDocId == id)
            .Select(reference =>
            {
                var docRef = docs.FirstOrDefault(x => x.Id == reference.ChildDocId);
                var primary = fields.Find(field => field.Id == docRef.PrimaryFieldId);
                var actual = fields.Find(field => field.Id == docRef.ActualFieldId);

                return new DocWithStatusModel
                {
                    DocId = docRef.Id,
                    Designation = actual.Designation ?? primary.Designation,
                    Status = primary.Status
                };
            })
            .ToList();
        
        var result = new DocumentWithFieldsModel
        {
            Primary = await _fieldsRepository.GetByIdAsync(doc.PrimaryFieldId),
            Actual = await _fieldsRepository.GetByIdAsync(doc.ActualFieldId.Value),
            DocId = doc.Id,
            References = references
        };

        return new OkObjectResult(result);
    }

    public async Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId)
    {
        if (parameters.Name is not null)
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        var docs = await _docsRepository.GetDocumentsAsync(parameters, isValid, limit, lastId);
        var fields = await _fieldsRepository.GetFieldsByDocIds(docs.Select(x => x.Id).ToList());
        var docsWithFields = docs.AsParallel().Select(doc => new DocumentWithFieldsModel
        {
            Primary = fields.FirstOrDefault(f => f.DocId == doc.Id && f.IsPrimary),
            Actual = fields.FirstOrDefault(f => f.DocId == doc.Id && !f.IsPrimary),
            DocId = doc.Id
        }).ToList();

        return docsWithFields;
    }
    
    public async Task<List<ShortInfoDocumentModel>> SearchValidAsync(SearchParametersModel parameters, int limit, int offset)
    {
        if (parameters.Name is not null)
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        var response = new SearchResponse<DocumentESModel>();
        if (parameters.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => pi.GetValue(parameters))
            .All(value => value is null))
        {
            response = await _searchRepository.SearchAllAsync(limit, offset);
        }
        else
        {
            response = await _searchRepository.SearchValidFieldsAsync(parameters, limit, offset);
        }

        var docResults = new Dictionary<long, ShortInfoDocumentModel>();
        var maxScore = response.HitsMetadata.MaxScore;
        
        foreach (var hit in response.Hits)
        {
            var fieldEntity = hit.Source?.Field;
            Console.WriteLine($"{maxScore}, {hit.Score.Value}, {maxScore / hit.Score.Value * 5}");
            if (docResults.ContainsKey(fieldEntity.DocId))
                continue;
            docResults[fieldEntity.DocId] = new ShortInfoDocumentModel
            {
                CodeOKS = fieldEntity.CodeOKS, Designation = fieldEntity.Designation, FullName = fieldEntity.FullName,
                Id = fieldEntity.DocId, RelevanceMark = Convert.ToInt32(hit.Score.Value / maxScore * 5), ApplicationArea = fieldEntity.ApplicationArea
            };
        }
        
        return docResults.Values.ToList();
    }
    
    public async Task<int> GetDocumentsCountAsync(SearchParametersModel parameters, bool? isValid)
    {
        if (parameters.Name is not null)
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        return await _docsRepository.GetCountOfDocumentsAsync(parameters, isValid);
    }

    public async Task<List<DocWithGeneralInfoModel>> GetDocsWithGeneralInfoAsync()
    {
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();

        return docs.Select(doc =>
            {
                var primary = fields.Find(field => field.Id == doc.PrimaryFieldId);
                var actual = fields.Find(field => field.Id == doc.ActualFieldId);
                
                return new DocWithGeneralInfoModel
                {
                    Id = doc.Id, Designation = actual.Designation ?? primary?.Designation, FullName = actual.FullName ?? primary?.FullName,
                    ApplicationArea = actual.ApplicationArea ?? primary?.ApplicationArea, CodeOKS = actual.CodeOKS ?? primary?.CodeOKS
                };
            })
            .ToList();
    }

    public async Task<IActionResult> UploadFileForDocumentAsync(UploadFileModel file, long docId)
    {
        await _filesRepository.UploadFileAsync(file.File, file.Extension, docId);
        var doc = await _docsRepository.GetByIdAsync(docId);
        var primary = await _fieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        primary.DocumentText = $"https://gost-storage.ru/documents/{primary.Designation}.{file.Extension}";
        await _fieldsRepository.UpdateAsync(primary);
        return new OkResult();
    }

    public async Task IndexAllDocumentsAsync()
    {
        var docs = GetDocumentsAsync(new SearchParametersModel(), true, 100000, 0).Result;
        await _searchRepository.IndexAllDocumentsAsync(docs);

    }

    public async Task IndexDocumentDataAsync(IFormFile file, long docId)
    {
        var s = new MemoryStream();
        await file.CopyToAsync(s);
        await _searchRepository.IndexDocumentDataAsync(Convert.ToBase64String(s.ToArray()), docId);
    }
}