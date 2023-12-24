using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Concrete;

public class FieldsService(IFieldsRepository fieldsRepository, IReferencesRepository referencesRepository, IDocsRepository docsRepository) : IFieldsService
{
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    private readonly IDocsRepository _docsRepository = docsRepository;

    public IActionResult Update(FieldEntity updatedField, long docId)
    {
        var doc = _docsRepository.GetById(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        updatedField.Id = doc.PrimaryFieldId;
        _fieldsRepository.Update(updatedField);

        return new OkObjectResult("Document updated successfully.");
    }

    public IActionResult Actualize(FieldEntity actualizedField, long docId)
    {
        var doc = _docsRepository.GetById(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        actualizedField.Id = doc.ActualFieldId.Value;
        
        _fieldsRepository.Update(actualizedField);

        return new OkObjectResult("Document actualized successfully");
    }
}