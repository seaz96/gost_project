using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Interfaces;

public interface IDocsService
{
    Task<long> AddNewDocAsync(FieldEntity primaryField);

    Task<IActionResult> DeleteDocAsync(long id);

    Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status);
    
    Task<DocumentWithFieldsModel?> GetDocumentAsync(long id);

    Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(
        SearchParametersModel parameters,
        bool? isValid,
        int limit,
        int lastId);

    Task<int> GetDocumentsCountAsync(SearchParametersModel parameters, bool? isValid);

    Task<IActionResult> UploadFileForDocumentAsync(UploadFileRequest file, long docId);

    Task<List<ShortInfoDocumentModel>> SearchAsync(FtsSearchQuery query);

    Task<List<ShortInfoDocumentModel>> SearchAllAsync(int limit, int offset);

    Task IndexAllDocumentsAsync();
}