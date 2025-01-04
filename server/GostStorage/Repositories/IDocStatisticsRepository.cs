using GostStorage.Entities;

namespace GostStorage.Repositories;

public interface IDocStatisticsRepository
{
    public Task<List<UserAction>> GetAllAsync();

    public Task AddAsync(UserAction statistic);

    public Task DeleteAsync(long docId);
}