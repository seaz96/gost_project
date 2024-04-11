using GostStorage.Domain.Entities;
using GostStorage.Domain.Models;

namespace GostStorage.Domain.Repositories;

public interface IDocsRepository
{
    public Task<List<DocEntity>> GetAllAsync();

    public Task<List<DocEntity>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit,
        int lastId);

    public Task<int> GetCountOfDocumentsAsync(SearchParametersModel parameters, bool? isValid);

    public Task<DocEntity?> GetByIdAsync(long id);

    public Task<IList<DocWithGeneralInfoModel>> GetDocsIdByDesignationAsync(List<string> docDesignations);

    public Task<long> AddAsync(DocEntity document);

    public Task DeleteAsync(long id);
}