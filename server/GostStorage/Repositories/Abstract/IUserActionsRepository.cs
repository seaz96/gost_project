using GostStorage.Entities;
using GostStorage.Models.Statistic;

namespace GostStorage.Repositories.Abstract;

public interface IUserActionsRepository
{
    Task<List<DocumentViewsResponse>> GetViewsAsync(GetViewsModel model);

    Task<List<UserActionDocumentModel>> GetActionsAsync(DocumentCountRequest model);

    public Task AddAsync(UserAction statistic);

    public Task DeleteAsync(long docId);
}