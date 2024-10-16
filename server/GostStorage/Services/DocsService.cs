using Elastic.Clients.Elasticsearch;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using GostStorage.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services;

public class DocsService(
    IDocsRepository docsRepository,
    IFieldsRepository fieldsRepository,
    IReferencesRepository referencesRepository,
    IFieldsService fieldsService,
    IFilesRepository filesRepository,
    ISearchRepository searchRepository) : IDocsService
{
    public async Task<long> AddNewDocAsync(FieldEntity primaryField)
    {
        if (primaryField.Designation is not null) primaryField.Designation = TextFormattingHelper.FormatDesignation(primaryField.Designation);
        var doc = await docsRepository.GetByDesignationAsync(primaryField.Designation);

        if (doc is not null)
        {
            await fieldsService.UpdateAsync(primaryField, doc.Id);
            await ChangeStatusAsync(doc.Id, primaryField.Status);
            return doc.Id;
        }

        var actualField = new FieldEntity
        {
            LastEditTime = DateTime.UtcNow,
            Status = primaryField.Status
        };
        var primaryId = await fieldsRepository.AddAsync(primaryField);
        var actualId = await fieldsRepository.AddAsync(actualField);

        doc = new DocEntity { ActualFieldId = actualId, PrimaryFieldId = primaryId };

        var docId = await docsRepository.AddAsync(doc);

        if (docId == 0) await Task.Delay(1000);

        primaryField.DocId = docId;
        actualField.DocId = docId;

        await fieldsRepository.UpdateAsync(primaryField);
        await fieldsRepository.UpdateAsync(actualField);

        IndexAllDocumentsAsync();

        return docId;
    }

    public async Task<IActionResult> DeleteDocAsync(long id)
    {
        var doc = await docsRepository.GetByIdAsync(id);

        if (doc is null) return new UnprocessableEntityObjectResult($"Document with id {id} not found.");

        await fieldsRepository.DeleteAsync(doc.PrimaryFieldId);
        await fieldsRepository.DeleteAsync(doc.ActualFieldId);

        await docsRepository.DeleteAsync(id);

        IndexAllDocumentsAsync();

        return new OkObjectResult("Document deleted successfully.");
    }

    public async Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status)
    {
        var doc = await docsRepository.GetByIdAsync(id);

        if (doc is null) return new UnprocessableEntityObjectResult($"Document with id {id} not found.");

        var primaryField = await fieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        var actualField = await fieldsRepository.GetByIdAsync(doc.ActualFieldId);

        if (primaryField is not null)
        {
            primaryField.Status = status;
            await fieldsRepository.UpdateAsync(primaryField);
        }

        if (actualField is not null)
        {
            actualField.Status = status;
            await fieldsRepository.UpdateAsync(actualField);
        }

        IndexAllDocumentsAsync();

        return new OkObjectResult("Status changed successfully.");
    }

    public async Task<ActionResult<DocumentWithFieldsModel>> GetDocumentAsync(long id)
    {
        var doc = await docsRepository.GetByIdAsync(id);

        if (doc is null) return new UnprocessableEntityObjectResult($"Document with id {id} not found.");

        var docs = await docsRepository.GetAllAsync();
        var fields = await fieldsRepository.GetAllAsync();

        var references = (await referencesRepository.GetAllAsync())
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
            Primary = await fieldsRepository.GetByIdAsync(doc.PrimaryFieldId),
            Actual = await fieldsRepository.GetByIdAsync(doc.ActualFieldId.Value),
            DocId = doc.Id,
            References = references
        };

        return new OkObjectResult(result);
    }

    public async Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId)
    {
        if (parameters.Name is not null) parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);

        var docs = await docsRepository.GetDocumentsAsync(parameters, isValid, limit, lastId);
        var fields = await fieldsRepository.GetFieldsByDocIds(docs.Select(x => x.Id).ToList());
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
        if (parameters.Name is not null) parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);

        var response = new SearchResponse<DocumentESModel>();
        if (parameters.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => pi.GetValue(parameters))
            .All(value => value is null))
            response = await searchRepository.SearchAllAsync(limit, offset);
        else
            response = await searchRepository.SearchValidFieldsAsync(parameters, limit, offset);

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
        if (parameters.Name is not null) parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);

        return await docsRepository.GetCountOfDocumentsAsync(parameters, isValid);
    }

    public async Task<List<DocWithGeneralInfoModel>> GetDocsWithGeneralInfoAsync()
    {
        var docs = await docsRepository.GetAllAsync();
        var fields = await fieldsRepository.GetAllAsync();

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
        await filesRepository.UploadFileAsync(file.File, file.Extension, docId);
        var doc = await docsRepository.GetByIdAsync(docId);
        var primary = await fieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        primary.DocumentText = $"https://gost-storage.ru/documents/{primary.Designation}.{file.Extension}";
        await fieldsRepository.UpdateAsync(primary);
        return new OkResult();
    }

    public async Task IndexAllDocumentsAsync()
    {
        var docs = GetDocumentsAsync(new SearchParametersModel(), true, 100000, 0).Result;
        await searchRepository.IndexAllDocumentsAsync(docs);
    }

    public async Task IndexDocumentDataAsync(IFormFile file, long docId)
    {
        var s = new MemoryStream();
        await file.CopyToAsync(s);
        await searchRepository.IndexDocumentDataAsync(Convert.ToBase64String(s.ToArray()), docId);
    }
}