using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;

namespace GostStorage.Repositories.Abstract;

public interface IDocumentsRepository
{
    public Task<int> GetCountOfDocumentsAsync(SearchQuery? parameters);

    public Task<Document?> GetByIdAsync(long id);

    public Task<Document?> GetByDesignationAsync(string designation);
    
    Task<FullDocument?> GetDocumentWithFields(long docId);

    public Task<IList<Document>> GetDocsIdByDesignationAsync(List<string> docDesignations);

    Task<Document?> GetDocumentByDesignationAsync(string designation);

    public Task<long> AddAsync(Document document);

    public Task DeleteAsync(long id);

    public Task UpdateStatusAsync(long id, DocumentStatus status);

    Task<List<FullDocument>> GetDocumentsWithFields(SearchQuery? parameters);

    Task<int> CountDocumentsWithFields(SearchQuery? parameters);
}