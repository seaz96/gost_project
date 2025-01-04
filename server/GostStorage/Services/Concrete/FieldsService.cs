using AutoMapper;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Repositories;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Concrete;

public class FieldsService(
        IPrimaryFieldsRepository primaryFieldsRepository,
        IActualFieldsRepository actualFieldsRepository,
        IDocumentsRepository documentsRepository,
        IMapper mapper)
    : IFieldsService
{
    public async Task<IActionResult> UpdateAsync(Field updatedField, long docId)
    {
        updatedField.Designation = TextFormattingHelper.FormatDesignation(updatedField.Designation);

        var doc = await documentsRepository.GetByIdAsync(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        updatedField.Id = doc.PrimaryFieldId;
        await primaryFieldsRepository.UpdateAsync(mapper.Map<PrimaryField>(updatedField));

        return new OkObjectResult("Document updated successfully.");
    }

    public async Task<IActionResult> ActualizeAsync(Field actualizedField, long docId)
    {
        actualizedField.Designation = TextFormattingHelper.FormatDesignation(actualizedField.Designation);
        var doc = await documentsRepository.GetByIdAsync(docId);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {docId} not found.");
        }
        
        actualizedField.Id = doc.ActualFieldId;
        
        await actualFieldsRepository.UpdateAsync(mapper.Map<ActualField>(actualizedField));

        return new OkObjectResult("Document actualized successfully");
    }
}