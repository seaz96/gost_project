using Elastic.Clients.Elasticsearch;
using GostStorage.Models.Docs;

namespace GostStorage.Repositories.Interfaces;

public interface ISearchRepository
{
    public Task<SearchResponse<DocumentESModel>> SearchValidFieldsAsync(SearchParametersModel parameters, int limit, int offset);
    
    public Task<SearchResponse<DocumentESModel>> SearchCanceledFieldsAsync(SearchParametersModel parameters, int limit, int offset);

    public Task<SearchResponse<DocumentESModel>> SearchAllAsync(int limit, int offset);

    public Task IndexAllDocumentsAsync(List<DocumentWithFieldsModel> docs);

    public Task IndexDocument(DocumentESModel document);

    public Task IndexDocumentDataAsync(string data, long docId);

    public Task DeleteDocumentAsync(long docId);
}