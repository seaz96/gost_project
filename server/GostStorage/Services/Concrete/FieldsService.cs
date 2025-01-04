using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Repositories;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Concrete;

public class FieldsService(
        IFieldsRepository fieldsRepository,
        IReferencesRepository referencesRepository,
        IDocsRepository docsRepository)
    : IFieldsService
{
    public async Task<IActionResult> UpdateAsync(Field updatedField, long docId)
    {
        if (updatedField.Designation is not null)
        {
            updatedField.Designation = TextFormattingHelper.FormatDesignation(updatedField.Designation);
        }

        var doc = await docsRepository.GetByIdAsync(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        updatedField.Id = doc.PrimaryFieldId;
        await fieldsRepository.UpdateAsync(updatedField);

        return new OkObjectResult("Document updated successfully.");
    }

    public async Task<IActionResult> ActualizeAsync(Field actualizedField, long docId)
    {
        if (actualizedField.Designation is not null)
        {
            actualizedField.Designation = TextFormattingHelper.FormatDesignation(actualizedField.Designation);
        }
        var doc = await docsRepository.GetByIdAsync(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        actualizedField.Id = doc.ActualFieldId;
        
        await fieldsRepository.UpdateAsync(actualizedField);

        return new OkObjectResult("Document actualized successfully");
    }
}