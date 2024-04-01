﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GostStorage.Domain.Navigations;
using GostStorage.Domain.Repositories;
using GostStorage.Services.Models.Accounts;
using GostStorage.Services.Services.Abstract;

namespace GostStorage.API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(IPasswordHasher passwordHasher, IAccountService accountService) : ControllerBase
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccountService _accountService = accountService;

    /// <summary>
    /// Log in to account
    /// </summary>
    /// <returns>User info</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        return await _accountService.LoginAsync(loginModel);
    }
    /// <summary>
    /// Create a new account and log in
    /// </summary>
    /// <returns>Return user info with auth token</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        return await _accountService.RegisterAsync(registerModel);
    }

    /// <summary>
    /// Change user password by admin
    /// </summary>
    [HttpPost("restore-password")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> RestorePassword([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        return await _accountService.RestorePasswordAsync(passwordRestoreModel);
    }

    /// <summary>
    /// Change password by old password
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel passwordChangeModel)
    {
        return await _accountService.ChangePasswordAsync(passwordChangeModel);
    }

    /// <summary>
    /// Get list of all users for admin
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet("list")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> UsersList()
    {
        return await _accountService.GetUsersListAsync();
    }
    
    /// <summary>
    /// Get full user info
    /// </summary>
    [HttpGet("get-user-info")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> GetUserInfo([FromQuery] long id)
    {
        return await _accountService.GetUserInfoAsync(id);
    }

    /// <summary>
    /// Edit own user info 
    /// </summary>
    [Authorize]
    [HttpPost("self-edit")]
    public async Task<IActionResult> SelfEdit([FromBody] UserSelfEditModel userSelfEditModel)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null || !int.TryParse(idClaim, out var id))
        {
            return Unauthorized();
        }

        return await _accountService.SelfEditAsync(userSelfEditModel, id);
    }

    /// <summary>
    /// Edit user info by admin
    /// </summary>
    [HttpPost("admin-edit")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<IActionResult> AdminEdit([FromBody] UserAdminEditModel userAdminEditModel)
    {
        return await _accountService.AdminEditAsync(userAdminEditModel, User);
    }

    /// <summary>
    /// Make user admin
    /// </summary>
    [HttpPost("make-admin")]
    [Authorize(Roles = "Heisenberg")]
    public async Task<IActionResult> MakeAdmin(ChangeUserRoleModel requestModel)
    {
        return await _accountService.MakeAdminAsync(requestModel);
    }
    
    /// <summary>
    /// Get self user info (authorized only)
    /// </summary>
    [Authorize]
    [HttpGet("self-info")]
    public async Task<IActionResult> GetSelfInfo()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        return await _accountService.GetSelfInfoAsync(userId);
    }
}
