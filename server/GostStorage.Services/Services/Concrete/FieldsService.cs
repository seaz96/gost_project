using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Services.Helpers;
using GostStorage.Services.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Services.Concrete;

public class FieldsService(IFieldsRepository fieldsRepository, IReferencesRepository referencesRepository,
    IDocsRepository docsRepository) : IFieldsService
{
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    private readonly IDocsRepository _docsRepository = docsRepository;

    public async Task<IActionResult> UpdateAsync(FieldEntity updatedField, long docId)
    {
        if (updatedField.Designation is not null)
        {
            updatedField.Designation = TextFormattingHelper.FormatDesignation(updatedField.Designation);
        }

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
        if (actualizedField.Designation is not null)
        {
            actualizedField.Designation = TextFormattingHelper.FormatDesignation(actualizedField.Designation);
        }
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