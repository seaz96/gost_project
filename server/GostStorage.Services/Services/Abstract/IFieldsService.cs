using GostStorage.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Services.Abstract;

public interface IFieldsService
{
    public Task<IActionResult> UpdateAsync(FieldEntity updatedField, long docId);

    public Task<IActionResult> ActualizeAsync(FieldEntity actualizedField, long docId);
}