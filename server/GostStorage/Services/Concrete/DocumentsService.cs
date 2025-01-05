using AutoMapper;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using GostStorage.Repositories;
using GostStorage.Repositories.Abstract;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GostStorage.Services.Concrete;

public class DocumentsService(
        IDocumentsRepository documentsRepository,
        IPrimaryFieldsRepository primaryFieldsRepository,
        IActualFieldsRepository actualFieldsRepository,
        IReferencesRepository referencesRepository,
        IFieldsService fieldsService,
        IFilesRepository filesRepository,
        ISearchRepository searchRepository,
        IMapper mapper)
    : IDocumentsService
{
    public async Task<long> AddDocumentAsync(PrimaryField primaryField, DocumentStatus status)
    {
        primaryField.Designation = TextFormattingHelper.FormatDesignation(primaryField.Designation);
        var doc = await documentsRepository.GetByDesignationAsync(primaryField.Designation);
        
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
        
        var docId = await documentsRepository.AddAsync(doc);
        
        // костыль: возможно еще не присвоился айди для документа, нужно подождать
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
        var doc = await documentsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return false;
        }
        
        await primaryFieldsRepository.DeleteAsync(doc.PrimaryFieldId);
        await primaryFieldsRepository.DeleteAsync(doc.ActualFieldId);
        
        await documentsRepository.DeleteAsync(id);
        await searchRepository.DeleteDocumentAsync(id);

        return true;
    }

    public async Task<IActionResult> ChangeStatusAsync(long id, DocumentStatus status)
    {
        await documentsRepository.UpdateStatusAsync(id, status);
        await searchRepository.ChangeDocumentStatusAsync(id, status);

        return new OkObjectResult("Status changed successfully.");
    }

    public async Task<FullDocument?> GetDocumentAsync(long id)
    {
        var doc = await documentsRepository.GetDocumentWithFields(id);
        
        if (doc is null)
        {
            return null;
        }

        var references = await referencesRepository.GetDocumentsByParentIdAsync(id);
        doc.References = references;

        return doc;
    }

    public async Task<List<FullDocument>> GetDocumentsAsync(GetDocumentRequest? parameters)
    {
        if (!string.IsNullOrEmpty(parameters.Name))
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        return await documentsRepository.GetDocumentsWithFields(parameters);
    }
    
    public async Task<int> GetDocumentsCountAsync(GetDocumentRequest? parameters)
    {
        if (parameters.Name is not null)
        {
            parameters.Name = TextFormattingHelper.FormatDesignation(parameters.Name);
        }

        return await documentsRepository.GetCountOfDocumentsAsync(parameters);
    }

    //todo: исправить хардкод бы
    public async Task<IActionResult> UploadFileForDocumentAsync(UploadFileModel file, long docId)
    {
        await filesRepository.UploadFileAsync(file.File, file.Extension, docId);
        var doc = await documentsRepository.GetByIdAsync(docId);
        var primary = await primaryFieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        primary.DocumentText = $"https://gost-storage.ru/documents/{doc.Designation}.{file.Extension}";
        await primaryFieldsRepository.UpdateAsync(primary);
        return new OkResult();
    }
    
    public async Task<List<ShortInfoDocumentModel>> SearchAsync(SearchQuery query)
    {
        var result = await searchRepository.SearchAsync(query);
        
        return result.Select(mapper.Map<ShortInfoDocumentModel>).ToList();
    }

    public async Task IndexAllDocumentsAsync()
    {
        var documents = await documentsRepository.GetDocumentsWithFields(null);

        foreach (var documnet in documents)
        {
            Log.Logger.Information($"indexing {documnet.DocId}");
            await searchRepository.IndexDocumentAsync(new SearchIndexModel
            {
                Document = SearchHelper.SplitFieldsToIndexDocument(documnet.DocId, documnet.Primary, documnet.Actual)
            });
        }
    }
}