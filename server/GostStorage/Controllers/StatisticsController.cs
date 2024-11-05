using System.Security.Claims;
using GostStorage.Entities;
using GostStorage.Models.Stats;
using GostStorage.Navigations;
using GostStorage.Repositories.Interfaces;
using GostStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/stats")]
public class StatisticsController(
        IDocStatisticsService docStatisticsService,
        IUsersRepository usersRepository)
    : ControllerBase
{
    [HttpGet("get-views")]
    public async Task<IActionResult> GetViewsAsync([FromQuery] GetViewsModel model)
    {
        return await docStatisticsService.GetViews(model);
    }

    [HttpGet("get-count")]
    public async Task<IActionResult> GetCountOfDocsAsync([FromQuery] GetCountOfDocsModel model)
    {
        return await docStatisticsService.GetCount(model);
    }

    [Authorize]
    [HttpPost("update-views/{docId}")]
    public async Task<IActionResult> UpdateViews(long docId)
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await usersRepository.GetUserAsync(userId);

        await docStatisticsService.AddAsync(new DocStatisticEntity
        {
            OrgBranch = user!.OrgBranch,
            Action = ActionType.View,
            DocId = docId,
            Date = DateTime.UtcNow,
            UserId = userId
        });

        return Ok("Views updated succesfully!");
    }
}