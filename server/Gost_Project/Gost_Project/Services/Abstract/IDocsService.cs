using Gost_Project.Data.Entities;

namespace Gost_Project.Services.Abstract;

public interface IDocsService
{
    public long AddNewDoc(FieldEntity primaryField);

    public void DeleteDoc(long id);
}