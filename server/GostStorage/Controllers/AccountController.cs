using System.Security.Claims;
using GostStorage.Models.Accounts;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        return await accountService.LoginAsync(loginModel);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        return await accountService.RegisterAsync(registerModel);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("restore-password")]
    public async Task<IActionResult> RestorePassword([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        return await accountService.RestorePasswordAsync(passwordRestoreModel);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel passwordChangeModel)
    {
        return await accountService.ChangePasswordAsync(passwordChangeModel);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpGet("list")]
    public async Task<IActionResult> UsersList()
    {
        return await accountService.GetUsersListAsync();
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpGet("get-user-info")]
    public async Task<IActionResult> GetUserInfo([FromQuery] long id)
    {
        return await accountService.GetUserInfoAsync(id);
    }

    [Authorize]
    [HttpPost("self-edit")]
    public async Task<IActionResult> SelfEdit([FromBody] UserSelfEditModel userSelfEditModel)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null || !int.TryParse(idClaim, out var id)) return Unauthorized();

        return await accountService.SelfEditAsync(userSelfEditModel, id);
    }

    [Authorize(Roles = "Heisenberg,Admin")]
    [HttpPost("admin-edit")]
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
 
    [Authorize]
    [HttpGet("self-info")]
    public async Task<IActionResult> GetSelfInfo()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        return await accountService.GetSelfInfoAsync(userId);
    }
}