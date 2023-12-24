using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

public interface IDocsService
{
    public long AddNewDoc(FieldEntity primaryField);

    public void DeleteDoc(long id);

    public IActionResult ChangeStatus(long id, DocStatuses status);
}