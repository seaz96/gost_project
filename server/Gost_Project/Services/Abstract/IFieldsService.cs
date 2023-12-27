using Gost_Project.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

public interface IFieldsService
{
    public Task<IActionResult> UpdateAsync(FieldEntity updatedField, long docId);

    public Task<IActionResult> ActualizeAsync(FieldEntity actualizedField, long docId);
}