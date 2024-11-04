using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GostStorage.Models.Accounts;
using GostStorage.Services.Interfaces;

namespace GostStorage.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(IPasswordHasher passwordHasher, IAccountService accountService) : ControllerBase
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        return await accountService.LoginAsync(loginModel);
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        return await accountService.RegisterAsync(registerModel);
    }

    [HttpPost("restore-password")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> RestorePassword([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        return await accountService.RestorePasswordAsync(passwordRestoreModel);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel passwordChangeModel)
    {
        return await accountService.ChangePasswordAsync(passwordChangeModel);
    }

    [HttpGet("list")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> UsersList()
    {
        return await accountService.GetUsersListAsync();
    }

    [HttpGet("get-user-info")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> GetUserInfo([FromQuery] long id)
    {
        return await accountService.GetUserInfoAsync(id);
    }

    [Authorize]
    [HttpPost("self-edit")]
    public async Task<IActionResult> SelfEdit([FromBody] UserSelfEditModel userSelfEditModel)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null || !int.TryParse(idClaim, out var id))
        {
            return Unauthorized();
        }

        return await accountService.SelfEditAsync(userSelfEditModel, id);
    }

    [HttpPost("admin-edit")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> AdminEdit([FromBody] UserAdminEditModel userAdminEditModel)
    {
        return await accountService.AdminEditAsync(userAdminEditModel, User);
    }

    [HttpPost("make-admin")]
    [Authorize(Roles = "Heisenberg")]
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