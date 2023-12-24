using Gost_Project.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

public interface IFieldsService
{
    public IActionResult Update(FieldEntity updatedField, long docId);

    public IActionResult Actualize(FieldEntity actualizedField, long docId);
}