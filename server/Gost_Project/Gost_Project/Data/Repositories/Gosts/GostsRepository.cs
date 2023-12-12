using System.Diagnostics.CodeAnalysis;
using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Gosts;

public class GostsRepository : IGostsRepository
{
    private readonly DataContext _context;

    public GostsRepository(DataContext context)
    {
        _context = context;
    }

    public List<GostEntity> GetAll()
    {
        return _context.Gosts.ToList();
    }

    public GostEntity GetById(long id)
    {
        return _context.Gosts.Where(gost => gost.Id == id).FirstOrDefault();
    }
    
    public long Add(GostEntity field)
    {
        long id = _context.Gosts.ToList().Count + 1;
        field.Id = id;
        _context.Gosts.Add(field);
        _context.SaveChanges();
        return id;
    }
}