using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Interfaces;

public interface IDocsService
{
    public Task<long> AddNewDocAsync(FieldEntity primaryField);

    public Task<IActionResult> DeleteDocAsync(long id);

    public Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status);

    public Task<ActionResult<DocumentWithFieldsModel>> GetDocumentAsync(long id);
    
    public Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId);

    public Task<List<ShortInfoDocumentModel>> SearchAsync(FtsSearchQuery parameters, int limit, int offset);
    public Task<List<ShortInfoDocumentModel>> SearchAllAsync(int limit, int offset);

    public Task<int> GetDocumentsCountAsync(SearchParametersModel parameters, bool? isValid);

    public Task<List<DocWithGeneralInfoModel>> GetDocsWithGeneralInfoAsync();

    public Task<IActionResult> UploadFileForDocumentAsync(UploadFileModel file, long docId);

    public Task IndexAllDocumentsAsync();

    public Task IndexDocumentDataAsync(IFormFile file, long docId);
}