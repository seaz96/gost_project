using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IDocsRepository
{
    public Task<List<DocEntity>> GetAllAsync();

    public Task<DocEntity?> GetByIdAsync(long id);

    public Task<long> AddAsync(DocEntity document);

    public Task DeleteAsync(long id);
}