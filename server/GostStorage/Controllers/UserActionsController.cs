using GostStorage.Models.Statistic;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/actions")]
public class UserActionsController(IUserActionsService userActionsService) : ControllerBase
{
    [Authorize]
    [HttpGet("views")]
    public async Task<IActionResult> GetViewsAsync([FromQuery] GetViewsModel model)
    {
        return await userActionsService.GetViews(model);
    }

    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> GetActionsAsync([FromQuery] DocumentCountRequest model)
    {
        return await userActionsService.GetActionsAsync(model);
    }
}