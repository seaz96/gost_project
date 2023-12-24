using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

public interface IDocsService
{
    public Task<long> AddNewDocAsync(FieldEntity primaryField);

    public Task<IActionResult> DeleteDocAsync(long id);

    public Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status);
}