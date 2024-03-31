using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Models;
using Gost_Project.Data.Models.Docs;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gost_Project.Data.Repositories.Concrete;

public class DocsRepository(DataContext context) : IDocsRepository
{
    private readonly DataContext _context = context;

    public async Task<List<DocEntity>> GetAllAsync()
    {
        return await _context.Docs.ToListAsync();
    }

    public async Task<List<DocEntity>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId)
    {
        var fieldIds = await SearchHelper.SearchFields(parameters, isValid, _context);

        var docs = _context.Docs
            .Where(x => fieldIds.Contains(x.PrimaryFieldId) || fieldIds.Contains(x.ActualFieldId.Value))
            .Distinct()
            .OrderBy(x => x.Id)
            .Where(x => x.Id > lastId)
            .Take(limit)
            .ToList();
        
        return docs;
    }
    
    public async Task<int> GetCountOfDocumentsAsync(SearchParametersModel parameters, bool? isValid)
    {
        var fieldIds = await SearchHelper.SearchFields(parameters, isValid, _context);

        return _context.Docs
            .Where(x => fieldIds.Contains(x.PrimaryFieldId) || fieldIds.Contains(x.ActualFieldId.Value))
            .Distinct()
            .Count();
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