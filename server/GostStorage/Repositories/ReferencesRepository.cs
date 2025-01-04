using GostStorage.Data;
using GostStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class ReferencesRepository(DataContext context) : IReferencesRepository
{
    public async Task<List<DocumentReference>> GetAllAsync()
    {
        return await context.References.ToListAsync();
    }

    public async Task<DocumentReference?> GetByIdAsync(long id)
    {
        return await context.References.Where(reference => reference.Id == id).FirstOrDefaultAsync();
    }

    public async Task<DocumentReference?> GetByParentIdAsync(long id)
    {
        return await context.References.FirstOrDefaultAsync(reference => reference.ParentalDocId == id);
    }

    public async Task<DocumentReference?> GetByChildIdAsync(long id)
    {
        return await context.References.FirstOrDefaultAsync(reference => reference.ChildDocId == id);
    }

    public async Task AddAsync(DocumentReference reference)
    {
        await context.References.AddAsync(reference);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<DocumentReference> references)
    {
        await context.References.AddRangeAsync(references);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllByParentIdAsync(long parentId)
    {
        await context.References.Where(reference => reference.ParentalDocId == parentId).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAllByChildIdAsync(long parentId)
    {
        await context.References.Where(reference => reference.ChildDocId == parentId).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }

    public async Task UpdateByParentIdAsync(List<long> referenceIds, long parentId)
    {
        var references = context.References.Where(reference => reference.ParentalDocId == parentId);
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

        await context.References.Where(reference =>
            reference.ParentalDocId == parentId && toDelete.Contains(reference.ChildDocId)).ExecuteDeleteAsync();
        
        await AddRangeAsync(referenceIds
            .Select(id => new DocumentReference
                {
                    ChildDocId = id, 
                    ParentalDocId = parentId
                })
            .ToList());
        
        await context.SaveChangesAsync();
    }
}