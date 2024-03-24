using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

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