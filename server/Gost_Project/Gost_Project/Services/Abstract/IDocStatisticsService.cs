using Gost_Project.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Abstract;

public interface IDocStatisticsService
{
    public Task AddNewDocStatsAsync(long docId);

    public Task UpdateViewsAsync(long docId);

    public Task UpdateChangedAsync(long docId);

    public Task DeleteAsync(long docId);

    public Task<ActionResult<List<DocWithViewsModel>>> GetViews(GetViewsModel model);

    public Task<IActionResult> GetCount(GetCountOfDocsModel model);
}