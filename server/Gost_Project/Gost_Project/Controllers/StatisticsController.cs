using Gost_Project.Data.Models;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/stats")]
public class StatisticsController(IDocStatisticsService docStatisticsService) : ControllerBase
{
    private readonly IDocStatisticsService _docStatisticsService = docStatisticsService;
    
    [HttpGet("get-views")]
    public async Task<ActionResult<List<DocWithViewsModel>>> GetViewsAsync([FromQuery] GetViewsModel model)
    {
        return await _docStatisticsService.GetViews(model);
    }
}