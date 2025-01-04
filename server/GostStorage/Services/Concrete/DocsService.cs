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
        IPrimaryFieldsRepository primaryFieldsRepository,
        IActualFieldsRepository actualFieldsRepository,
        IReferencesRepository referencesRepository,
        IFieldsService fieldsService,
        IFilesRepository filesRepository,
        ISearchRepository searchRepository,
        IMapper mapper)
    : IDocsService
{
    public async Task<long> AddDocumentAsync(PrimaryField primaryField, DocumentStatus status)
    {
        primaryField.Designation = TextFormattingHelper.FormatDesignation(primaryField.Designation);
        var doc = await docsRepository.GetByDesignationAsync(primaryField.Designation);
        
        if (doc is not null)
        {
            await fieldsService.UpdateAsync(primaryField, doc.Id);
            await ChangeStatusAsync(doc.Id, status);
            return doc.Id;
        }
        
        var actualField = new ActualField
        {
            Designation = primaryField.Designation,
            LastEditTime = DateTime.UtcNow
        };
        var primaryId = await primaryFieldsRepository.AddAsync(primaryField);
        var actualId = await actualFieldsRepository.AddAsync(actualField);
        
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
        
        await primaryFieldsRepository.UpdateAsync(primaryField);
        await actualFieldsRepository.UpdateAsync(actualField);

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
        
        await primaryFieldsRepository.DeleteAsync(doc.PrimaryFieldId);
        await primaryFieldsRepository.DeleteAsync(doc.ActualFieldId);
        
        await docsRepository.DeleteAsync(id);
        await searchRepository.DeleteDocumentAsync(id);

        return true;
    }

    //todo
    public async Task<IActionResult> ChangeStatusAsync(long id, DocumentStatus status)
    {
        await docsRepository.UpdateStatusAsync(id, status);
        
        /*var ftsDocument = mapper.Map<SearchDocument>(actualField);
        ftsDocument.Id = id;
        var indexModel = new SearchIndexModel { Document = ftsDocument };
        await searchRepository.IndexDocumentAsync(indexModel).ConfigureAwait(false);*/

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
        var fields = await primaryFieldsRepository.GetAllAsync();
        
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
                    Status = doc.Status
                };
            })
            .ToList();
        
        var result = new DocumentWithFieldsModel
        {
            Primary = await primaryFieldsRepository.GetByIdAsync(doc.PrimaryFieldId),
            Actual = await primaryFieldsRepository.GetByIdAsync(doc.ActualFieldId),
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
        var docsWithFields = docs.AsParallel().Select(async doc => new DocumentWithFieldsModel
        {
            Primary = await primaryFieldsRepository.GetByIdAsync(doc.PrimaryFieldId),
            Actual = await actualFieldsRepository.GetByIdAsync(doc.PrimaryFieldId),
            DocId = doc.Id
        }).ToList();

        return (await Task.WhenAll(docsWithFields)).ToList();
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
        var primary = await primaryFieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        primary.DocumentText = $"https://gost-storage.ru/documents/{primary.Designation}.{file.Extension}";
        await primaryFieldsRepository.UpdateAsync(primary);
        return new OkResult();
    }
    
    public async Task<List<ShortInfoDocumentModel>> SearchAsync(SearchQuery query)
    {
        var result = await searchRepository.SearchAsync(query);
        
        return result.Select(mapper.Map<ShortInfoDocumentModel>).ToList();
    }

    public async Task UpdateStatus(long docId, DocumentStatus status)
    {
        await docsRepository.UpdateStatusAsync(docId, status);
    }

    public async Task IndexAllDocumentsAsync()
    {
        var docs = await docsRepository.GetAllAsync();
        var fields = await primaryFieldsRepository.GetAllAsync();

        foreach (var doc in docs)
        {
            if (doc.Status is DocumentStatus.Inactive)
                continue;
            
            var primary = fields.FirstOrDefault(x => x.Id == doc.PrimaryFieldId);
            var actual = fields.FirstOrDefault(x => x.Id == doc.ActualFieldId);
            
            Log.Logger.Information($"indexing {doc.Id}");
            await searchRepository.IndexDocumentAsync(new SearchIndexModel
            {
                Document = SearchHelper.SplitFieldsToIndexDocument(doc.Id, primary, actual)
            });
        }
    }
}