using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

public interface IDocStatisticsService
{
    public Task AddAsync(DocStatisticEntity statistic);

    public Task DeleteAsync(long docId);

    public Task<IActionResult> GetViews(GetViewsModel model);

    public Task<IActionResult> GetCount(GetCountOfDocsModel model);
}