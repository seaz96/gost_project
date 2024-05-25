using GostStorage.Domain.Entities;
using GostStorage.Domain.Models;
using GostStorage.Domain.Navigations;
using GostStorage.Services.Models.Docs;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Services.Abstract;

public interface IDocsService
{
    public Task<long> AddNewDocAsync(FieldEntity primaryField);

    public Task<IActionResult> DeleteDocAsync(long id);

    public Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status);

    public Task<ActionResult<GetDocumentResponseModel>> GetDocumentAsync(long id);

    public Task<List<GetDocumentResponseModel>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId);

    public Task<int> GetDocumentsCountAsync(SearchParametersModel parameters, bool? isValid);

    public Task<List<DocWithGeneralInfoModel>> GetDocsWithGeneralInfoAsync();
}