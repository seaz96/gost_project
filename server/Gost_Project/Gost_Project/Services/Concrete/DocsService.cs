using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Concrete;

public class DocsService : IDocsService
{
    private readonly IDocsRepository _docsRepository;
    private readonly IFieldsRepository _fieldsRepository;

    public DocsService(IDocsRepository docsRepository, IFieldsRepository fieldsRepository)
    {
        _fieldsRepository = fieldsRepository;
        _docsRepository = docsRepository;
    }

    public long AddNewDoc(FieldEntity primaryField)
    {
        var actualField = new FieldEntity();

        var primaryId = _fieldsRepository.Add(primaryField);
        var actualId = _fieldsRepository.Add(actualField);

        var doc = new DocEntity { ActualFieldId = actualId, PrimaryFieldId = primaryId };
        var docId = _docsRepository.Add(doc);

        return docId;
    }

    public void DeleteDoc(long id)
    {
        var doc = _docsRepository.GetById(id);

        _fieldsRepository.Delete(doc.PrimaryFieldId);
        _fieldsRepository.Delete(doc.ActualFieldId);
        
        _docsRepository.Delete(id);
    }

    public IActionResult ChangeStatus(long id, DocStatuses status)
    {
        var doc = _docsRepository.GetById(id);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        var primaryField = _fieldsRepository.GetById(doc.PrimaryFieldId);
        var actualField = _fieldsRepository.GetById(doc.ActualFieldId);
        
        if (primaryField is not null)
        {
            primaryField.Status = status;
            _fieldsRepository.Update(primaryField);
        }
        if (actualField is not null)
        {
            actualField.Status = status;
            _fieldsRepository.Update(actualField);
        }

        return new OkObjectResult("Status changed successfully!");
    }
}