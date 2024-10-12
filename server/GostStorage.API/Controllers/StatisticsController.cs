using System.Security.Claims;
using GostStorage.API.Entities;
using GostStorage.API.Models.Stats;
using GostStorage.API.Navigations;
using GostStorage.API.Repositories.Interfaces;
using GostStorage.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.API.Controllers;

[ApiController]
[Route("api/stats")]
public class StatisticsController(IDocStatisticsService docStatisticsService, IUsersRepository usersRepository) : ControllerBase
{
    private readonly IDocStatisticsService _docStatisticsService = docStatisticsService;
    private readonly IUsersRepository _usersRepository = usersRepository;
    
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
    
    /// <summary>
    /// Updating document views
    /// </summary>
    [Authorize]
    [HttpPost("update-views/{docId}")]
    public async Task<IActionResult> UpdateViews(long docId)
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await _usersRepository.GetUserAsync(userId);

        await _docStatisticsService.AddAsync(new DocStatisticEntity { OrgBranch = user!.OrgBranch, Action = ActionType.View, DocId = docId, Date = DateTime.UtcNow, UserId = userId});

        return Ok("Views updated succesfully!");
    }
}