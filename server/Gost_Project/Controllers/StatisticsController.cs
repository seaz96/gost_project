using Gost_Project.Data.Models;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/stats")]
public class StatisticsController(IDocStatisticsService docStatisticsService) : ControllerBase
{
    private readonly IDocStatisticsService _docStatisticsService = docStatisticsService;
    
    /// <summary>
    /// Get views of every document bu filters
    /// </summary>
    /// <returns>List of document views with ids</returns>
    [HttpGet("get-views")]
    public async Task<IActionResult> GetViewsAsync([FromQuery] GetViewsModel model)
    {
        return await _docStatisticsService.GetViews(model);
    }

    /// <summary>
    /// Get count of all documents by filters
    /// </summary>
    /// <returns>Count of docs</returns>
    [HttpGet("get-count")]
    public async Task<IActionResult> GetCountOfDocsAsync([FromQuery] GetCountOfDocsModel model)
    {
        return await _docStatisticsService.GetCount(model);
    }
}