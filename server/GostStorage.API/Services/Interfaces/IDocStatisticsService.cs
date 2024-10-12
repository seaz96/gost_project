using GostStorage.API.Entities;
using GostStorage.API.Models.Stats;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.API.Services.Interfaces;

public interface IDocStatisticsService
{
    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);

    public Task<IActionResult> GetViews(GetViewsModel model);

    public Task<IActionResult> GetCount(GetCountOfDocsModel model);
}