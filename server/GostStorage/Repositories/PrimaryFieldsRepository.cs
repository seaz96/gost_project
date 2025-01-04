using GostStorage.Data;
using GostStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class PrimaryFieldsRepository(DataContext context) : IPrimaryFieldsRepository
{
    public async Task<List<PrimaryField>> GetAllAsync()
    {
        return await context.PrimaryFields.ToListAsync();
    }

    public PrimaryField? GetById(long? id)
    {
        return context.PrimaryFields.FirstOrDefault(field => field.Id == id);
    }

    public async Task<ICollection<PrimaryField>> GetFieldsByDocIds(ICollection<long> docIds)
    {
        return await context.PrimaryFields.Where(f => docIds.Contains(f.DocId)).ToListAsync();
    }

    public async Task<PrimaryField?> GetByIdAsync(long? id)
    {
        return await context.PrimaryFields.FirstOrDefaultAsync(field => field.Id == id);
    }

    public async Task<long> AddAsync(PrimaryField field)
    {
        await context.PrimaryFields.AddAsync(field);
        await context.SaveChangesAsync();
        return field.Id;
    }

    public async Task DeleteAsync(long? id)
    {
        await context.PrimaryFields.Where(field => field.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(PrimaryField newField)
    {
        context.PrimaryFields.Update(newField);

        await context.SaveChangesAsync();
    }
}