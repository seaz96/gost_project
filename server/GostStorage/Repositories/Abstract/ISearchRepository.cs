using GostStorage.Models.Search;
using GostStorage.Navigations;

namespace GostStorage.Repositories.Abstract;

public interface ISearchRepository
{
    public Task<List<SearchEntity>> SearchAsync(SearchQuery query);

    public Task IndexAllDocumentsAsync(List<SearchIndexModel> documents);

    public Task IndexDocumentAsync(SearchIndexModel document);

    public Task DeleteDocumentAsync(long docId);

    Task ChangeDocumentStatusAsync(long id, DocumentStatus status);
}