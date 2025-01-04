using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Abstract;

public interface IDocsService
{
    Task<long> AddDocumentAsync(Field primaryField);

    Task<bool> DeleteDocumentAsync(long id);

    Task<IActionResult> ChangeStatusAsync(long id, DocumentStatus status);
    
    Task<DocumentWithFieldsModel?> GetDocumentAsync(long id);

    Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(
        GetDocumentRequest parameters);

    Task<int> GetDocumentsCountAsync(GetDocumentRequest parameters);

    Task<IActionResult> UploadFileForDocumentAsync(UploadFileModel file, long docId);

    Task<List<ShortInfoDocumentModel>> SearchAsync(SearchQuery query);

    Task IndexAllDocumentsAsync();
}