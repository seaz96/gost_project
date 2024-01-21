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

    public Task<ActionResult<GetDocumentResponseModel>> GetDocument(long id);

    public Task<List<GetDocumentResponseModel>> GetAllDocuments();
    
    public Task<List<GetDocumentResponseModel>> GetAllDocuments(SearchParametersModel parameters);
    
    public Task<List<GetDocumentResponseModel>> GetAllDocuments(SearchParametersModel parameters, bool isValid);

    public Task<List<DocWithGeneralInfoModel>> GetDocsWithGeneralInfo();
}