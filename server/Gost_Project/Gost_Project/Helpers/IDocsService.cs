using Gost_Project.Data.Entities;

namespace Gost_Project.Helpers;

public interface IDocsService
{
    public long AddNewDoc(FieldEntity primaryField);
}