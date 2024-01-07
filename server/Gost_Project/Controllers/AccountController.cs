using Anomaly.Middlewares;
using CorsairMessengerServer;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Extensions;
using Gost_Project.Helpers;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(IPasswordHasher passwordHasher, IUsersRepository usersRepository) : ControllerBase
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    private readonly IUsersRepository _usersRepository = usersRepository;

    /// <summary>
    /// Log in to account, cookie auth
    /// </summary>
    /// <returns>User info</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
    {
        var user = await _usersRepository.GetUserAsync(loginModel.Login);

        if (user is null)
        {
            return BadRequest(new { Field = nameof(loginModel.Login) });
        }

        var verified = _passwordHasher.Verify(loginModel.Password, user.Password!);

        if (!verified)
        {
            return BadRequest(new { Field = nameof(loginModel.Password) });
        }

        var token = SecurityHelper.GetAuthToken(user);

        AddAuthorizationCookie(token);

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Name,
            role = user.Role.ToString()
        });
    }
    /// <summary>
    /// Create a new account and log in
    /// </summary>
    /// <returns>Add auth token to cookies and return user info</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var isLoginExist = await _usersRepository.IsLoginExistAsync(registerModel.Login);

        if (isLoginExist)
        {
            return Conflict(new { Field = nameof(registerModel.Login) });
        }

        var user = CreateUser(registerModel, _passwordHasher);

        var task = _usersRepository.AddAsync(user);

        var token = SecurityHelper.GetAuthToken(user);

        AddAuthorizationCookie(token);

        task.Wait();

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Name,
            role = user.Role.ToString()
        });
    }

    /// <summary>
    /// Change user password by admin
    /// </summary>
    [HttpPost("restore-password")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RestorePassword([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        var user = await _usersRepository.GetUserAsync(passwordRestoreModel.Login);

        if (user is null)
        {
            return BadRequest(new { Field = nameof(passwordRestoreModel.Login) });
        }

        user.Password = _passwordHasher.Hash(passwordRestoreModel.NewPassword);

        return Ok();
    }

    /// <summary>
    /// Change password by old password
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] PasswordChangeModel passwordChangeModel)
    {
        var user = await _usersRepository.GetUserAsync(passwordChangeModel.Login);

        if (user is null)
        {
            return BadRequest(new { Field = nameof(passwordChangeModel.Login) });
        }

        if (user.Password != _passwordHasher.Hash(passwordChangeModel.OldPassword))
        {
            return BadRequest("Old password is wrong");
        }
        
        user.Password = _passwordHasher.Hash(passwordChangeModel.NewPassword);

        return Ok();
    }

    /// <summary>
    /// Get list of all users for admin
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet("list")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UsersList()
    {
        var users = (await _usersRepository.GetAllAsync()).Select(user => new 
        {
            user.Name,
            user.Login,
            Role = user.Role.ToString(),
        });

        return Ok(users);
    }

    /// <summary>
    /// Edit own user info 
    /// </summary>
    [HttpPost("self-edit")]
    public async Task<ActionResult> SelfEdit([FromBody] UserSelfEditModel userSelfEditModel)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (idClaim is null || !int.TryParse(idClaim, out var id))
        {
            return Unauthorized();
        }

        var user = await _usersRepository.GetUserAsync(id);

        if (user is null)
        {
            return BadRequest();
        }

        user.OrgName = userSelfEditModel.OrgName ?? user.OrgName;
        user.OrgBranch = userSelfEditModel.OrgBranch ?? user.OrgBranch;
        user.OrgActivity = userSelfEditModel.OrgActivity ?? user.OrgActivity;

        return Ok();
    }

    /// <summary>
    /// Edit user info by admin
    /// </summary>
    [HttpPost("admin-edit")]
    public async Task<ActionResult> AdminEdit([FromBody] UserAdminEditModel userAdminEditModel)
    {
        var user = await _usersRepository.GetUserAsync(userAdminEditModel.Login);

        if (user is null)
        {
            return BadRequest(new { Field = nameof(userAdminEditModel.Login) });
        }

        user.OrgName = userAdminEditModel.OrgName ?? user.OrgName;
        user.OrgBranch = userAdminEditModel.OrgBranch ?? user.OrgBranch;
        user.OrgActivity = userAdminEditModel.OrgActivity ?? user.OrgActivity;
        user.Role = userAdminEditModel.IsAdmin ? UserRoles.Admin : UserRoles.User;

        return Ok();
    }

    private static UserEntity CreateUser(RegisterModel registerModel, IPasswordHasher hasher)
    {
        var hashedPassword = hasher.Hash(registerModel.Password);

        return new UserEntity 
        {
            Login = registerModel.Login,
            Name = registerModel.Name,
            Role = Enum.Parse<UserRoles>(registerModel.Role.FirstCharToUpperNextToLower()),
            Password = hashedPassword,
            OrgBranch = registerModel.OrgBranch,
            OrgActivity = registerModel.OrgActivity,
            OrgName = registerModel.OrgName
        };
    }

    private void AddAuthorizationCookie(string token)
    {
        HttpContext.Response.Cookies.Append(RequestHeadersComplementaryMiddleware.AuthorizationTokenCookieName, token, new CookieOptions
        {
            MaxAge = AuthOptions.AuthTokenLifetime,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
        });
    }
}
