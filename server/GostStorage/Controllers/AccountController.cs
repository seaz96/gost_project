using System.Security.Claims;
using GostStorage.Models.Accounts;
using GostStorage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
    {
        return await accountService.LoginAsync(loginModel);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel registerModel)
    {
        return await accountService.RegisterAsync(registerModel);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("restore-password")]
    public async Task<IActionResult> RestorePasswordAsync([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        return await accountService.RestorePasswordAsync(passwordRestoreModel);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] PasswordChangeModel passwordChangeModel)
    {
        return await accountService.ChangePasswordAsync(passwordChangeModel);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAccountsListAsync()
    {
        return await accountService.GetUsersListAsync();
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfoAsync([FromQuery] long id)
    {
        return await accountService.GetUserInfoAsync(id);
    }

    [Authorize]
    [HttpPost("self-edit")]
    public async Task<IActionResult> SelfEditAsync([FromBody] UserSelfEditModel userSelfEditModel)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null || !int.TryParse(idClaim, out var id)) return Unauthorized();

        return await accountService.SelfEditAsync(userSelfEditModel, id);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("admin-edit")]
    public async Task<IActionResult> AdminEditAsync([FromBody] UserAdminEditModel userAdminEditModel)
    {
        return await accountService.AdminEditAsync(userAdminEditModel, User);
    }

    [Authorize(Roles = "Heisenberg")]
    [HttpPost("update-admin")]
    public async Task<IActionResult> ChangeAdminStatusAsync(ChangeUserRoleModel requestModel)
    {
        return await accountService.ChangeAdminStatusAsync(requestModel);
    }
 
    [Authorize]
    [HttpGet("self-info")]
    public async Task<IActionResult> GetSelfInfoAsync()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        return await accountService.GetSelfInfoAsync(userId);
    }
}