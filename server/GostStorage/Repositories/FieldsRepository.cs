using GostStorage.Data;
using GostStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class FieldsRepository(DataContext context) : IFieldsRepository
{
    public async Task<List<Field>> GetAllAsync()
    {
        return await context.Fields.ToListAsync();
    }

    public Field? GetById(long? id)
    {
        return context.Fields.FirstOrDefault(field => field.Id == id);
    }

    public async Task<ICollection<Field>> GetFieldsByDocIds(ICollection<long> docIds)
    {
        return await context.Fields.Where(f => docIds.Contains(f.DocId)).ToListAsync();
    }

    public async Task<Field?> GetByIdAsync(long? id)
    {
        return await context.Fields.FirstOrDefaultAsync(field => field.Id == id);
    }

    public async Task<long> AddAsync(Field field)
    {
        await context.Fields.AddAsync(field);
        await context.SaveChangesAsync();
        return field.Id;
    }

    public async Task DeleteAsync(long? id)
    {
        await context.Fields.Where(field => field.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Field newField)
    {
        context.Fields.Update(newField);

        await context.SaveChangesAsync();
    }
}