using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories.Concrete;

public class ReferencesRepository(DataContext context) : IReferencesRepository
{
    public async Task<List<DocReferenceEntity>> GetAllAsync()
    {
        return await context.DocsReferences.ToListAsync();
    }

    public async Task<DocReferenceEntity?> GetByIdAsync(long id)
    {
        return await context.DocsReferences.Where(reference => reference.Id == id).FirstOrDefaultAsync();
    }

    public async Task<DocReferenceEntity?> GetByParentIdAsync(long id)
    {
        return await context.DocsReferences.FirstOrDefaultAsync(reference => reference.ParentalDocId == id);
    }

    public async Task<DocReferenceEntity?> GetByChildIdAsync(long id)
    {
        return await context.DocsReferences.FirstOrDefaultAsync(reference => reference.ChildDocId == id);
    }

    public async Task AddAsync(DocReferenceEntity reference)
    {
        await context.DocsReferences.AddAsync(reference);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<DocReferenceEntity> references)
    {
        await context.DocsReferences.AddRangeAsync(references);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllByParentIdAsync(long parentId)
    {
        await context.DocsReferences.Where(reference => reference.ParentalDocId == parentId).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAllByChildIdAsync(long parentId)
    {
        await context.DocsReferences.Where(reference => reference.ChildDocId == parentId).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }

    public async Task UpdateByParentIdAsync(List<long> referenceIds, long parentId)
    {
        var references = context.DocsReferences.Where(reference => reference.ParentalDocId == parentId);
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

        await context.DocsReferences.Where(reference =>
            reference.ParentalDocId == parentId && toDelete.Contains(reference.ChildDocId)).ExecuteDeleteAsync();
        
        await AddRangeAsync(referenceIds
            .Select(id => new DocReferenceEntity
                {
                    ChildDocId = id, 
                    ParentalDocId = parentId
                })
            .ToList());
        
        await context.SaveChangesAsync();
    }
}