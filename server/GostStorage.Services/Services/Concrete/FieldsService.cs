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
        updatedField.Designation = TextFormattingHelper.FormatDesignation(updatedField.Designation);
        var doc = await _docsRepository.GetByIdAsync(docId);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"???????? ? ??????????????? {docId} ?? ??????");
        }

        updatedField.Id = doc.PrimaryFieldId;
        await _fieldsRepository.UpdateAsync(updatedField);

        return new OkObjectResult("???????? ??????? ????????");
    }

    public async Task<IActionResult> ActualizeAsync(FieldEntity actualizedField, long docId)
    {
        actualizedField.Designation = TextFormattingHelper.FormatDesignation(actualizedField.Designation);
        var doc = await _docsRepository.GetByIdAsync(docId);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"???????? ? ???????????????  {docId} ?? ??????");
        }

        actualizedField.Id = doc.ActualFieldId.Value;

        await _fieldsRepository.UpdateAsync(actualizedField);

        return new OkObjectResult("???????? ??????? ??????????????");
    }
}