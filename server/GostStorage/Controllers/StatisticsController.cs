using System.Security.Claims;
using GostStorage.Entities;
using GostStorage.Models.Statistic;
using GostStorage.Navigations;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/stats")]
public class StatisticsController(IDocStatisticsService docStatisticsService) : ControllerBase
{
    [HttpGet("get-views")]
    public async Task<IActionResult> GetViewsAsync([FromQuery] GetViewsModel model)
    {
        return await docStatisticsService.GetViews(model);
    }

    [HttpGet("get-count")]
    public async Task<IActionResult> GetCountOfDocsAsync([FromQuery] DocumentCountRequest model)
    {
        return await docStatisticsService.GetCount(model);
    }
}