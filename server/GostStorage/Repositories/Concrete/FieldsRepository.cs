using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories.Concrete;

public class FieldsRepository(DataContext context) : IFieldsRepository
{
    public async Task<List<FieldEntity>> GetAllAsync()
    {
        return await context.Fields.ToListAsync();
    }

    public FieldEntity? GetById(long? id)
    {
        return context.Fields.FirstOrDefault(field => field.Id == id);
    }

    public async Task<ICollection<FieldEntity>> GetFieldsByDocIds(ICollection<long> docIds)
    {
        return await context.Fields.Where(f => docIds.Contains(f.DocId)).ToListAsync();
    }

    public async Task<FieldEntity?> GetByIdAsync(long? id)
    {
        return await context.Fields.FirstOrDefaultAsync(field => field.Id == id);
    }

    public async Task<long> AddAsync(FieldEntity field)
    {
        await context.Fields.AddAsync(field);
        await context.SaveChangesAsync();
        return field.Id;
    }

    public async Task DeleteAsync(long? id)
    {
        await context.Fields.Where(field => field.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(FieldEntity newField)
    {
        context.Fields.Update(newField);

        await context.SaveChangesAsync();
    }
}