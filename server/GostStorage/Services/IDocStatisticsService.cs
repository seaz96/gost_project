using GostStorage.Entities;
using GostStorage.Models.Stats;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services;

public interface IDocStatisticsService
{
    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);

    public Task<IActionResult> GetViews(GetViewsModel model);

    public Task<IActionResult> GetCount(GetCountOfDocsModel model);
}