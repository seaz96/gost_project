using GostStorage.Entities;
using GostStorage.Models.Statistic;

namespace GostStorage.Repositories;

public interface IUserActionsRepository
{
    public Task<List<UserAction>> GetAllAsync();

    Task<List<DocumentViewsResponse>> GetViewsAsync(GetViewsModel model);

    public Task AddAsync(UserAction statistic);

    public Task DeleteAsync(long docId);
}