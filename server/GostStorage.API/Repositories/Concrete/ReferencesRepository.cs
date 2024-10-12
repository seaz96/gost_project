using GostStorage.API.Data;
using GostStorage.API.Entities;
using GostStorage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.API.Repositories.Concrete;

public class ReferencesRepository(DataContext context) : IReferencesRepository
{
    private readonly DataContext _context = context;

    public async Task<List<DocReferenceEntity>> GetAllAsync()
    {
        return await _context.DocsReferences.ToListAsync();
    }

    public async Task<DocReferenceEntity?> GetByIdAsync(long id)
    {
        return await _context.DocsReferences.Where(reference => reference.Id == id).FirstOrDefaultAsync();
    }

    public async Task<DocReferenceEntity?> GetByParentIdAsync(long id)
    {
        return await _context.DocsReferences.FirstOrDefaultAsync(reference => reference.ParentalDocId == id);
    }

    public async Task<DocReferenceEntity?> GetByChildIdAsync(long id)
    {
        return await _context.DocsReferences.FirstOrDefaultAsync(reference => reference.ChildDocId == id);
    }

    public async Task AddAsync(DocReferenceEntity reference)
    {
        await _context.DocsReferences.AddAsync(reference);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<DocReferenceEntity> references)
    {
        await _context.DocsReferences.AddRangeAsync(references);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllByParentIdAsync(long parentId)
    {
        await _context.DocsReferences.Where(reference => reference.ParentalDocId == parentId).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAllByChildIdAsync(long parentId)
    {
        await _context.DocsReferences.Where(reference => reference.ChildDocId == parentId).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task UpdateByParentIdAsync(List<long> referenceIds, long parentId)
    {
        var references = _context.DocsReferences.Where(reference => reference.ParentalDocId == parentId);
        var toDelete = new List<long>();
        
        foreach (var reference in references)
        {
            if (referenceIds.Contains(reference.ChildDocId))
            {
                referenceIds.Remove(reference.ChildDocId);
            }
            else
            {
                referenceIds.Remove(reference.ChildDocId);
                toDelete.Add(reference.ChildDocId);
            }
        }

        await _context.DocsReferences.Where(reference =>
            reference.ParentalDocId == parentId && toDelete.Contains(reference.ChildDocId)).ExecuteDeleteAsync();
        
        await AddRangeAsync(referenceIds.Select(id => new DocReferenceEntity {ChildDocId = id, ParentalDocId = parentId}).ToList());
        
        await _context.SaveChangesAsync();
    }
}