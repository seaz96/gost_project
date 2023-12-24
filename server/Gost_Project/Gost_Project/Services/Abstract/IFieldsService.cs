using Gost_Project.Data.Entities;

namespace Gost_Project.Services.Abstract;

public interface IFieldsService
{
    public void Update(FieldEntity updatedField, long docId);
}