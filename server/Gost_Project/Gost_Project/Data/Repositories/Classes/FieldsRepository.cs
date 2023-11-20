using Gost_Project.Data.Context;
using Gost_Project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gost_Project.Data.Repositories;

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
        long id = _context.Fields.ToList().Count + 1;
        field.Id = id;
        _context.Fields.Add(field);
        _context.SaveChanges();
        return id;
    }
}