using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using GostStorage.Domain.Entities;
using GostStorage.Domain.Entities.Base;
using GostStorage.Domain.Models;
using GostStorage.Services.Models.Docs;

namespace GostStorage.Domain.Repositories;

public interface ISearchRepository
{
    public Task<SearchResponse<DocumentESModel>> SearchValidFieldsAsync(SearchParametersModel parameters, int limit, int offset);
    
    public Task<SearchResponse<DocumentESModel>> SearchCanceledFieldsAsync(SearchParametersModel parameters, int limit, int offset);

    public Task<SearchResponse<DocumentESModel>> SearchAllAsync(int limit, int offset);

    public Task IndexAllDocumentsAsync(List<DocumentWithFieldsModel> docs);

    public Task IndexDocumentDataAsync(string data, long docId);
}