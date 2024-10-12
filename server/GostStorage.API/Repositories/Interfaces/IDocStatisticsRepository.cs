using GostStorage.API.Entities;

namespace GostStorage.API.Repositories.Interfaces;

public interface IDocStatisticsRepository
{
    public Task<List<DocStatisticEntity>> GetAllAsync();

    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);
}