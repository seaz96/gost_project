using GostStorage.Entities;
using GostStorage.Models.Statistic;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Abstract;

public interface IDocStatisticsService
{
    public Task AddAsync(UserAction statistic);

    public Task DeleteAsync(long docId);

    public Task<IActionResult> GetViews(GetViewsModel model);

    public Task<IActionResult> GetCount(DocumentCountRequest model);
}