using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using GostStorage.Domain.Entities;
using GostStorage.Domain.Entities.Base;
using GostStorage.Domain.Models;

namespace GostStorage.Domain.Repositories;

public interface ISearchRepository
{
    public Task<SearchResponse<FieldEntity>> SearchValidFieldsAsync<T>(SearchParametersModel parameters, int limit, int offset);
    
    public Task<SearchResponse<FieldEntity>> SearchCanceledFieldsAsync<T>(SearchParametersModel parameters, int limit, int offset);

    public Task IndexAllAsync();
}