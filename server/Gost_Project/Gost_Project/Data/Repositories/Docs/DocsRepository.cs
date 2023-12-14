using System.Diagnostics.CodeAnalysis;
using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Docs;

public class DocsRepository : IDocsRepository
{
    private readonly DataContext _context;

    public DocsRepository(DataContext context)
    {
        _context = context;
    }

    public List<DocEntity> GetAll()
    {
        return _context.Docs.ToList();
    }

    public DocEntity GetById(long id)
    {
        return _context.Docs.Where(gost => gost.Id == id).FirstOrDefault();
    }
    
    public long Add(DocEntity field)
    {
        long id = _context.Docs.ToList().Count + 1;
        field.Id = id;
        _context.Docs.Add(field);
        _context.SaveChanges();
        return id;
    }
}