using GostStorage.Entities;
using GostStorage.Models.Docs;

namespace GostStorage.Repositories;

public interface IDocsRepository
{
    public Task<List<Document>> GetAllAsync();

    public Task<List<Document>> GetDocumentsAsync(GetDocumentRequest parameters);

    public Task<int> GetCountOfDocumentsAsync(GetDocumentRequest parameters);

    public Task<Document?> GetByIdAsync(long id);

    public Task<Document?> GetByDesignationAsync(string designation);

    public Task<IList<DocWithGeneralInfoModel>> GetDocsIdByDesignationAsync(List<string> docDesignations);

    public Task<long> AddAsync(Document document);

    public Task DeleteAsync(long id);
}