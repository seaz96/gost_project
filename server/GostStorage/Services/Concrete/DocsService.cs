using AutoMapper;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using GostStorage.Repositories;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GostStorage.Services.Concrete;

public class DocsService(
        IDocsRepository docsRepository,
        IFieldsRepository fieldsRepository,
        IReferencesRepository referencesRepository,
        IFieldsService fieldsService,
        IFilesRepository filesRepository,
        ISearchRepository searchRepository,
        IMapper mapper)
    : IDocsService
{
    public async Task<long> AddDocumentAsync(Field primaryField)
    {
        primaryField.Designation = TextFormattingHelper.FormatDesignation(primaryField.Designation);
        var doc = await docsRepository.GetByDesignationAsync(primaryField.Designation);
        
        if (doc is not null)
        {
            await fieldsService.UpdateAsync(primaryField, doc.Id);
            await ChangeStatusAsync(doc.Id, primaryField.Status);
            return doc.Id;
        }
        
        var actualField = new Field
        {
            Designation = primaryField.Designation,
            LastEditTime = DateTime.UtcNow,
            Status = primaryField.Status
        };
        var primaryId = await fieldsRepository.AddAsync(primaryField);
        var actualId = await fieldsRepository.AddAsync(actualField);
        
        doc = new Document
        {
            Designation = primaryField.Designation,
            ActualFieldId = actualId,
            PrimaryFieldId = primaryId
        };
        
        var docId = await docsRepository.AddAsync(doc);
        
        if (docId == 0)
        {
            await Task.Delay(1000);
        }
        
        primaryField.DocId = docId;
        actualField.DocId = docId; 
        
        await fieldsRepository.UpdateAsync(primaryField);
        await fieldsRepository.UpdateAsync(actualField);

        var ftsDocument = mapper.Map<SearchDocument>(primaryField);
        ftsDocument.Id = docId;
        var indexModel = new SearchIndexModel { Document = ftsDocument };
        await searchRepository.IndexDocumentAsync(indexModel).ConfigureAwait(false);

        return docId;
    }

    public async Task<bool> DeleteDocumentAsync(long id)
    {
        var doc = await docsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return false;
        }
        
        await fieldsRepository.DeleteAsync(doc.PrimaryFieldId);
        await fieldsRepository.DeleteAsync(doc.ActualFieldId);
        
        await docsRepository.DeleteAsync(id);
        await searchRepository.DeleteDocumentAsync(id);

        return true;
    }

    public async Task<IActionResult> ChangeStatusAsync(long id, DocumentStatus status)
    {
        var doc = await docsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
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
        
        var ftsDocument = mapper.Map<SearchDocument>(actualField);
        ftsDocument.Id = id;
        var indexModel = new SearchIndexModel { Document = ftsDocument };
        await searchRepository.IndexDocumentAsync(indexModel).ConfigureAwait(false);

        return new OkObjectResult("Status changed successfully.");
    }

    public async Task<DocumentWithFieldsModel?> GetDocumentAsync(long id)
    {
        var doc = await docsRepository.GetByIdAsync(id);
        
        if (doc is null)
        {
            return null;
        }
        
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
            Actual = await fieldsRepository.GetByIdAsync(doc.ActualFieldId),
            DocId = doc.Id,
            References = references
        };

        return result;
    }

    public async Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(GetDocumentRequest parameters)
    {
        if (parameters.Name is not null)
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        var docs = await docsRepository.GetDocumentsAsync(parameters);
        var fields = await fieldsRepository.GetFieldsByDocIds(docs.Select(x => x.Id).ToList());
        var docsWithFields = docs.AsParallel().Select(doc => new DocumentWithFieldsModel
        {
            Primary = fields.FirstOrDefault(f => f.DocId == doc.Id && f.IsPrimary),
            Actual = fields.FirstOrDefault(f => f.DocId == doc.Id && !f.IsPrimary),
            DocId = doc.Id
        }).ToList();

        return docsWithFields;
    }
    
    public async Task<int> GetDocumentsCountAsync(GetDocumentRequest parameters)
    {
        if (parameters.Name is not null)
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        return await docsRepository.GetCountOfDocumentsAsync(parameters);
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
    
    public async Task<List<ShortInfoDocumentModel>> SearchAsync(SearchQuery query)
    {
        var result = await searchRepository.SearchAsync(query);
        
        return result.Select(mapper.Map<ShortInfoDocumentModel>).ToList();
    }

    public async Task IndexAllDocumentsAsync()
    {
        var docs = await docsRepository.GetAllAsync();
        var fields = await fieldsRepository.GetAllAsync();

        foreach (var doc in docs)
        {
            var primary = fields.FirstOrDefault(x => x.Id == doc.PrimaryFieldId);
            var actual = fields.FirstOrDefault(x => x.Id == doc.ActualFieldId);
            
            if (primary.Status is DocumentStatus.Inactive)
                continue;
            
            Log.Logger.Information($"indexing {doc.Id}");
            await searchRepository.IndexDocumentAsync(new SearchIndexModel
            {
                Document = SearchHelper.SplitFieldsToIndexDocument(doc.Id, primary, actual)
            });
        }
    }
}