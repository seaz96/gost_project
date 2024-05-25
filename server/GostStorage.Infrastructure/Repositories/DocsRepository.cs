using GostStorage.Domain.Entities;
using GostStorage.Domain.Models;
using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Helpers;
using GostStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Infrastructure.Repositories;

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

    public async Task<List<DocWithGeneralInfoModel>> GetDocumentsByDesignationAsync(IList<string> designations)
    {
        var docs = _context.Fields
            .Join(_context.Docs,
                f => f.DocId,
                d => d.Id,
                (f, d) => new { d.Id, f.Designation })
            .Where(x => designations.Contains(x.Designation))
            .Select(x => new { x.Id, x.Designation })
            .Distinct()
            .ToList();

        return _context.Docs
            .Where(x => docs.Any(f => f.Id == x.PrimaryFieldId) || docs.Any(f => f.Id == x.ActualFieldId.Value))
            .Distinct()
            .Select(x => new DocWithGeneralInfoModel
            {
                Id = x.Id,
                Designation = docs.FirstOrDefault(f => f.Id == x.PrimaryFieldId).Designation
            })
            .ToList();
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

    public async Task<DocEntity?> GetByDesignationAsync(string designation)
    {
        return (await _context.Docs.Join(_context.Fields,
                doc => doc.Id,
                field => field.DocId,
                (doc, field) => new { Doc = doc, Field = field })
            .FirstOrDefaultAsync(x => designation == x.Field.Designation))?.Doc;
    }

    public async Task<IList<DocWithGeneralInfoModel>> GetDocsIdByDesignationAsync(List<string> docDesignations)
    {
        return await _context.Docs.Join(_context.Fields,
                doc => doc.Id,
                field => field.DocId,
                (doc, field) => new { doc.Id, field.Designation })
                    .Where(doc => docDesignations.Contains(doc.Designation))
                    .GroupBy(doc => doc.Id)
                    .Select(group => new DocWithGeneralInfoModel { Id = group.First().Id, Designation = group.First().Designation })
                    .AsSingleQuery()
                    .ToListAsync();
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