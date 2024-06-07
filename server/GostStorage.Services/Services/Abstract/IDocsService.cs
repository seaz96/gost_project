using GostStorage.Domain.Entities;
using GostStorage.Domain.Models;
using GostStorage.Domain.Navigations;
using GostStorage.Services.Models.Docs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Services.Abstract;

public interface IDocsService
{
    public Task<long> AddNewDocAsync(FieldEntity primaryField);

    public Task<IActionResult> DeleteDocAsync(long id);

    public Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status);

    public Task<ActionResult<DocumentWithFieldsModel>> GetDocumentAsync(long id);
    
    public Task<List<DocumentWithFieldsModel>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId);

    public Task<List<ShortInfoDocumentModel>> SearchValidAsync(SearchParametersModel parameters, int limit, int offset);

    public Task<int> GetDocumentsCountAsync(SearchParametersModel parameters, bool? isValid);

    public Task<List<DocWithGeneralInfoModel>> GetDocsWithGeneralInfoAsync();

    public Task<IActionResult> UploadFileForDocumentAsync(UploadFileModel file, long docId);

    public Task IndexAllDocumentsAsync();

    public Task IndexDocumentDataAsync(IFormFile file, long docId);
}