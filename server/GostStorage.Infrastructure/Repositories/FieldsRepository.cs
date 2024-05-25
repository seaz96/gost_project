using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Infrastructure.Repositories;

public class FieldsRepository(DataContext context) : IFieldsRepository
{
    private readonly DataContext _context = context;

    public async Task<List<FieldEntity>> GetAllAsync()
    {
        return await _context.Fields.ToListAsync();
    }

    public FieldEntity? GetById(long? id)
    {
        return _context.Fields.FirstOrDefault(field => field.Id == id);
    }

    public async Task<ICollection<FieldEntity>> GetFieldsByDocIds(ICollection<long> docIds)
    {
        return await _context.Fields.Where(f => docIds.Contains(f.DocId)).ToListAsync();
    }

    public async Task<FieldEntity?> GetByIdAsync(long? id)
    {
        return await _context.Fields.FirstOrDefaultAsync(field => field.Id == id);
    }

    public async Task<long> AddAsync(FieldEntity field)
    {
        await _context.Fields.AddAsync(field);
        await _context.SaveChangesAsync();
        return field.Id;
    }

    public async Task DeleteAsync(long? id)
    {
        await _context.Fields.Where(field => field.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(FieldEntity newField)
    {
        _context.Fields.Update(newField);

        await _context.SaveChangesAsync();
    }
}