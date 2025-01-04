using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Navigations;

namespace GostStorage.Repositories.Abstract;

public interface IDocumentsRepository
{
    public Task<int> GetCountOfDocumentsAsync(GetDocumentRequest? parameters);

    public Task<Document?> GetByIdAsync(long id);

    public Task<Document?> GetByDesignationAsync(string designation);
    
    Task<FullDocument?> GetDocumentWithFields(long docId);

    public Task<IList<DocWithGeneralInfoModel>> GetDocsIdByDesignationAsync(List<string> docDesignations);

    public Task<long> AddAsync(Document document);

    public Task DeleteAsync(long id);

    public Task UpdateStatusAsync(long id, DocumentStatus status);

    Task<List<FullDocument>> GetDocumentsWithFields(GetDocumentRequest? parameters);
}