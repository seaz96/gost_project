using GostStorage.Models.Search;

namespace GostStorage.Repositories;

public interface ISearchRepository
{
    public Task<List<SearchEntity>> SearchAsync(SearchQuery query);

    public Task IndexAllDocumentsAsync(List<SearchIndexModel> documents);

    public Task IndexDocumentAsync(SearchIndexModel document);

    public Task DeleteDocumentAsync(long docId);
}