using Gost_Project.Data.Entities;

namespace Gost_Project.Helpers;

public interface IGostsService
{
    public long AddNewGost(FieldEntity primaryField);
}