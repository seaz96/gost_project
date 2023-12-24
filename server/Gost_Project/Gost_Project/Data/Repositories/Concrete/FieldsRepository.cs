using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Gost_Project.Data.Repositories.Abstract;

public class FieldsRepository : IFieldsRepository
{
    private readonly DataContext _context;

    public FieldsRepository(DataContext context)
    {
        _context = context;
    }

    public List<FieldEntity> GetAll()
    {
        return _context.Fields.ToList();
    }

    public FieldEntity GetById(long id)
    {
        return GetAll().Where(field => field.Id == id).FirstOrDefault();
    }

    public long Add(FieldEntity field)
    {
        _context.Fields.Add(field);
        _context.SaveChanges();
        return field.Id;
    }

    public void Delete(long? id)
    {
        _context.Fields.Where(field => field.Id == id).ExecuteDelete();
    }

    public void Update(FieldEntity newField)
    {
        _context.Fields.Update(newField);
        
        _context.SaveChanges();
    }
}