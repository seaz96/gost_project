using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using GostStorage.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories.Concrete;

public class DocumentsRepository(DataContext context) : IDocumentsRepository
{
    public Task<int> GetCountOfDocumentsAsync(SearchQuery? parameters)
    {
        return SearchHelper.GetFullDocumentQueryable(context)
            .ApplyFilters(parameters)
            .Where(x =>
                (parameters == null
                 || parameters.SearchFilters == null
                 || parameters.SearchFilters.Status == null
                 || x.Status == parameters.SearchFilters.Status)
                && x.Status != DocumentStatus.Inactive)
            .CountAsync();
    }

    public Task<Document?> GetByIdAsync(long id)
    {
        return context.Documents.FirstOrDefaultAsync(doc => doc.Id == id);
    }

    public Task<Document?> GetByDesignationAsync(string designation)
    {
        return context.Documents.FirstOrDefaultAsync(x => x.Designation == designation);
    }

    public async Task<IList<Document>> GetDocsIdByDesignationAsync(List<string> docDesignations)
    {
        return await context.Documents
            .Where(doc => docDesignations.Contains(doc.Designation))
            .AsSingleQuery()
            .ToListAsync();
    }

    public Task<Document?> GetDocumentByDesignationAsync(string designation)
    {
        return context.Documents
            .FirstOrDefaultAsync(doc => doc.Designation == designation);
    }

    public Task<FullDocument?> GetDocumentWithFields(long docId)
    {
        return SearchHelper.GetFullDocumentQueryable(context)
            .Where(x => x.Id == docId)
            .FirstOrDefaultAsync();
    }

    public Task<List<FullDocument>> GetDocumentsWithFields(SearchQuery? parameters)
    {
        return SearchHelper.GetFullDocumentQueryable(context)
            .ApplyFilters(parameters)
            .Where(x =>
                (parameters == null
                 || parameters.SearchFilters == null
                 || parameters.SearchFilters.Status == null
                 || x.Status == parameters.SearchFilters.Status)
                && x.Status != DocumentStatus.Inactive)
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToListAsync();
    }

    public Task<int> CountDocumentsWithFields(SearchQuery? parameters)
    {
        return SearchHelper.GetFullDocumentQueryable(context)
            .ApplyFilters(parameters)
            .Where(x =>
                (parameters == null
                 || parameters.SearchFilters == null
                 || parameters.SearchFilters.Status == null
                 || x.Status == parameters.SearchFilters.Status)
                && x.Status != DocumentStatus.Inactive)
            .CountAsync();
    }

    public async Task<long> AddAsync(Document document)
    {
        var d = await context.Documents.AddAsync(document);
        await context.SaveChangesAsync();
        
        return d.Entity.Id;
    }

    public async Task DeleteAsync(long id)
    {
        await context.Documents.Where(doc => doc.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateStatusAsync(long id, DocumentStatus status)
    {
        var document = await context.Documents.FirstOrDefaultAsync(d => d.Id == id);

        if (document is null) return;

        document.Status = status;
        context.Documents.Update(document);
        await context.SaveChangesAsync();
    }
}