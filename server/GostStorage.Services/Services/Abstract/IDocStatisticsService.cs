using GostStorage.Domain.Entities;
using GostStorage.Services.Models.Stats;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Services.Abstract;

public interface IDocStatisticsService
{
    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);

    public Task<IActionResult> GetViews(GetViewsModel model);

    public Task<IActionResult> GetCount(GetCountOfDocsModel model);
}