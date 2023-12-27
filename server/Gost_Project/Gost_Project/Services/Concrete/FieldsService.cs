using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Concrete;

public class FieldsService(IFieldsRepository fieldsRepository, IReferencesRepository referencesRepository,
    IDocsRepository docsRepository) : IFieldsService
{
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    private readonly IDocsRepository _docsRepository = docsRepository;

    public async Task<IActionResult> UpdateAsync(FieldEntity updatedField, long docId)
    {
        var doc = await _docsRepository.GetByIdAsync(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        updatedField.Id = doc.PrimaryFieldId;
        await _fieldsRepository.UpdateAsync(updatedField);

        return new OkObjectResult("Document updated successfully.");
    }

    public async Task<IActionResult> ActualizeAsync(FieldEntity actualizedField, long docId)
    {
        var doc = await _docsRepository.GetByIdAsync(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        actualizedField.Id = doc.ActualFieldId.Value;
        
        await _fieldsRepository.UpdateAsync(actualizedField);

        return new OkObjectResult("Document actualized successfully");
    }
}