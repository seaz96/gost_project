using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IDocStatisticsRepository
{
    public Task<List<DocStatisticEntity>> GetAllAsync();

    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);
}