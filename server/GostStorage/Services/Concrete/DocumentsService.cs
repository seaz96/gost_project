using AutoMapper;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
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
            PrimaryFieldId = primaryId,
            Status = status
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

    public async Task<List<FullDocument>> GetDocumentsAsync(SearchQuery? parameters)
    {
        if (!string.IsNullOrEmpty(parameters?.Text))
        {
            parameters.Text = TextFormattingHelper.FormatDesignation(parameters.Text);
        }

        return await documentsRepository.GetDocumentsWithFields(parameters);
    }

    public async Task<int> GetDocumentsCountAsync(SearchQuery? parameters)
    {
        if (parameters?.Text is not null)
        {
            parameters.Text = TextFormattingHelper.FormatDesignation(parameters.Text);
        }

        return await documentsRepository.GetCountOfDocumentsAsync(parameters);
    }

    //todo: исправить хардкод бы
    public async Task<bool> UploadFileForDocumentAsync(IFormFile file, long docId)
    {
        var filename = await filesRepository.UploadFileAsync(file, docId);

        if (filename is null) return false;

        var doc = await documentsRepository.GetByIdAsync(docId);
        var primary = await primaryFieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        primary.DocumentText = $"https://gost-storage.ru/documents/{filename}";
        await primaryFieldsRepository.UpdateAsync(primary);
        return true;
    }

    public async Task<List<GeneralDocumentInfoModel>> SearchAsync(SearchQuery query)
    {
        var result = await searchRepository.SearchAsync(query);

        return result.Select(mapper.Map<GeneralDocumentInfoModel>).ToList();
    }

    public Task<int> SearchCountAsync(SearchQuery query)
    {
        return searchRepository.CountAsync(query);
    }

    public async Task IndexAllDocumentsAsync()
    {
        var documents = await documentsRepository.GetDocumentsWithFields(
            new SearchQuery
            {
                SearchFilters = new SearchFilters
                {
                    Status = DocumentStatus.Valid,
                },
                Limit = await documentsRepository.CountDocumentsWithFields(new SearchQuery
                {
                    SearchFilters = new SearchFilters
                    {
                        Status = DocumentStatus.Valid,
                    }
                }),
                Offset = 0
            });
        documents.AddRange(await documentsRepository.GetDocumentsWithFields(
            new SearchQuery
            {
                SearchFilters = new SearchFilters
                {
                    Status = DocumentStatus.Canceled,
                },
                Limit = await documentsRepository.CountDocumentsWithFields(new SearchQuery
                {
                    SearchFilters = new SearchFilters
                    {
                        Status = DocumentStatus.Canceled,
                    }
                }),
                Offset = 0
            }));
        documents.AddRange(await documentsRepository.GetDocumentsWithFields(
            new SearchQuery
            {
                SearchFilters = new SearchFilters
                {
                    Status = DocumentStatus.Replaced,
                },
                Limit = await documentsRepository.CountDocumentsWithFields(new SearchQuery
                {
                    SearchFilters = new SearchFilters
                    {
                        Status = DocumentStatus.Replaced,
                    },
                }),
                Offset = 0
            }));

        foreach (var documnet in documents)
        {
            Log.Logger.Information($"indexing {documnet.Id}");
            await searchRepository.IndexDocumentAsync(new SearchIndexModel
            {
                Document = SearchHelper.SplitFieldsToIndexDocument(documnet.Id, documnet.Primary, documnet.Actual)
            });
        }
    }
}