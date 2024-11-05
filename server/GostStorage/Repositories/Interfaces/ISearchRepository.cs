using GostStorage.Models.Docs;

namespace GostStorage.Repositories.Interfaces;

public interface ISearchRepository
{
    public Task<FtsSearchEntity> SearchAsync(FtsSearchQuery query);

    public Task<FtsSearchEntity> SearchAllAsync(int limit, int offset);

    public Task IndexAllDocumentsAsync(List<FtsIndexModel> documents);

    public Task IndexDocument(FtsIndexModel document);

    public Task DeleteDocumentAsync(long docId);
}