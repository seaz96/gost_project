using GostStorage.Entities;
using GostStorage.Models.Docs;

namespace GostStorage.Repositories;

public interface IDocsRepository
{
    public Task<List<DocEntity>> GetAllAsync();

    public Task<List<DocEntity>> GetDocumentsAsync(SearchParametersModel parameters, bool? isValid, int limit,
        int lastId);

    public Task<int> GetCountOfDocumentsAsync(SearchParametersModel parameters, bool? isValid);

    public Task<DocEntity?> GetByIdAsync(long id);

    public Task<DocEntity?> GetByDesignationAsync(string designation);

    public Task<IList<DocWithGeneralInfoModel>> GetDocsIdByDesignationAsync(List<string> docDesignations);

    public Task<long> AddAsync(DocEntity document);

    public Task DeleteAsync(long id);
}