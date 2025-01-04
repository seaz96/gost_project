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

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel passwordChangeModel)
    {
        return await accountService.ChangePasswordAsync(passwordChangeModel);
    }

    [Authorize]
    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromBody] UserSelfEditModel userSelfEditModel)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null || !int.TryParse(idClaim, out var id)) return Unauthorized();

        return await accountService.SelfEditAsync(userSelfEditModel, id);
    }
 
    [Authorize]
    [HttpGet("self-info")]
    public async Task<IActionResult> GetSelfInfo()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        return await accountService.GetSelfInfoAsync(userId);
    }
}