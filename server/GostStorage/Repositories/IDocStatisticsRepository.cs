using GostStorage.Entities;

namespace GostStorage.Repositories;

public interface IDocStatisticsRepository
{
    public Task<List<DocStatisticEntity>> GetAllAsync();

    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);
}