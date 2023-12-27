using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Gost_Project.Data.Repositories.Concrete;

public class DocsRepository(DataContext context) : IDocsRepository
{
    private readonly DataContext _context = context;

    public async Task<List<DocEntity>> GetAllAsync()
    {
        return await _context.Docs.ToListAsync();
    }

    public async Task<DocEntity?> GetByIdAsync(long id)
    {
        return await _context.Docs.FirstOrDefaultAsync(doc => doc.Id == id);
    }

    public async Task<long> AddAsync(DocEntity document)
    {
        await _context.Docs.AddAsync(document);
        await _context.SaveChangesAsync();
        return document.Id;
    }

    public async Task DeleteAsync(long id)
    {
        await _context.Docs.Where(doc => doc.Id == id).ExecuteDeleteAsync();
    }
}