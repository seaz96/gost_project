using GostStorage.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.API.Services.Interfaces;

public interface IFieldsService
{
    public Task<IActionResult> UpdateAsync(FieldEntity updatedField, long docId);

    public Task<IActionResult> ActualizeAsync(FieldEntity actualizedField, long docId);
}