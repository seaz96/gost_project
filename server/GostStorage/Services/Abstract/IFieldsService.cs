using GostStorage.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Abstract;

public interface IFieldsService
{
    public Task<IActionResult> UpdateAsync(Field updatedField, long docId);

    public Task<IActionResult> ActualizeAsync(Field actualizedField, long docId);
}