using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IDocsRepository
{
    public Task<List<DocEntity>> GetAllAsync();

    public Task<List<DocEntity>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit, int lastId);

    public Task<int> GetCountOfDocumentsAsync(SearchParametersModel parameters, bool? isValid);

    public Task<DocEntity?> GetByIdAsync(long id);

    public Task<long> AddAsync(DocEntity document);

    public Task DeleteAsync(long id);
}