using GostStorage.Data;
using GostStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class ActualFieldsRepository(DataContext context) : IActualFieldsRepository
{
    public async Task<List<ActualField>> GetAllAsync()
    {
        return await context.ActualFields.ToListAsync();
    }

    public ActualField? GetById(long? id)
    {
        return context.ActualFields.FirstOrDefault(field => field.Id == id);
    }

    public async Task<ICollection<ActualField>> GetFieldsByDocIds(ICollection<long> docIds)
    {
        return await context.ActualFields.Where(f => docIds.Contains(f.DocId)).ToListAsync();
    }

    public async Task<ActualField?> GetByIdAsync(long? id)
    {
        return await context.ActualFields.FirstOrDefaultAsync(field => field.Id == id);
    }

    public async Task<long> AddAsync(ActualField field)
    {
        await context.ActualFields.AddAsync(field);
        await context.SaveChangesAsync();
        return field.Id;
    }

    public async Task DeleteAsync(long? id)
    {
        await context.ActualFields.Where(field => field.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(ActualField newField)
    {
        context.ActualFields.Update(newField);

        await context.SaveChangesAsync();
    }
}