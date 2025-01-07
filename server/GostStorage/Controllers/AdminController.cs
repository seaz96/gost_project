using GostStorage.Models.Accounts;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController(IAccountService accountService) : ControllerBase
{

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("restore-password")]
    public async Task<IActionResult> RestorePassword([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        return await accountService.RestorePasswordAsync(passwordRestoreModel);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> UsersList([FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (limit < 0 || offset < 0)
            return new BadRequestObjectResult("Limit or offset cannot be negative");
        
        return await accountService.GetUsersListAsync(limit, offset);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpGet("users/{id:long}")]
    public async Task<IActionResult> GetUserInfo(long id)
    {
        return await accountService.GetUserInfoAsync(id);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("edit-user")]
    public async Task<IActionResult> AdminEdit([FromBody] UserAdminEditModel userAdminEditModel)
    {
        return await accountService.AdminEditAsync(userAdminEditModel, User);
    }

    [Authorize(Roles = "Heisenberg")]
    [HttpPost("make-admin")]
    public async Task<IActionResult> MakeAdmin(ChangeUserRoleModel requestModel)
    {
        return await accountService.MakeAdminAsync(requestModel);
    }
}