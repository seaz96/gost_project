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
    /// Log in to account
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

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Name,
            user.OrgName,
            user.OrgBranch,
            user.OrgActivity,
            role = user.Role.ToString(),
            token
        });
    }
    /// <summary>
    /// Create a new account and log in
    /// </summary>
    /// <returns>Return user info with auth token</returns>
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

        task.Wait();

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Name,
            user.OrgName,
            user.OrgBranch,
            user.OrgActivity,
            role = user.Role.ToString(),
            token
        });
    }

    /// <summary>
    /// Change user password by admin
    /// </summary>
    [HttpPost("restore-password")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<ActionResult> RestorePassword([FromBody] PasswordRestoreModel passwordRestoreModel)
    {
        var user = await _usersRepository.GetUserAsync(passwordRestoreModel.Login);

        if (user is null)
        {
            return BadRequest(new { Field = nameof(passwordRestoreModel.Login) });
        }

        await _usersRepository.UpdatePasswordAsync(user.Id, _passwordHasher.Hash(passwordRestoreModel.NewPassword));

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

        if (!_passwordHasher.Verify(passwordChangeModel.OldPassword, user.Password!))
        {
            return BadRequest("Old password is wrong");
        }
        
        await _usersRepository.UpdatePasswordAsync(user.Id, _passwordHasher.Hash(passwordChangeModel.NewPassword));

        return Ok();
    }

    /// <summary>
    /// Get list of all users for admin
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet("list")]
    [Authorize(Roles = "Admin,Heisenberg")]
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

        await _usersRepository.UpdateAsync(user);

        return Ok();
    }

    /// <summary>
    /// Edit user info by admin
    /// </summary>
    [HttpPost("admin-edit")]
    [Authorize(Roles = "Admin,Heisenberg")]
    public async Task<ActionResult> AdminEdit([FromBody] UserAdminEditModel userAdminEditModel)
    {
        var user = await _usersRepository.GetUserAsync(userAdminEditModel.Login);

        if (user is null)
        {
            return BadRequest(new { Field = nameof(userAdminEditModel.Login) });
        }
        
        if (user.Role != UserRoles.User && User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value != "Heisenberg")
        {
            return new BadRequestObjectResult("You don't have permission");
        }
        
        user.OrgName = userAdminEditModel.OrgName ?? user.OrgName;
        user.OrgBranch = userAdminEditModel.OrgBranch ?? user.OrgBranch;
        user.OrgActivity = userAdminEditModel.OrgActivity ?? user.OrgActivity;
        
        await _usersRepository.UpdateAsync(user);

        return Ok();
    }

    /// <summary>
    /// Make user admin
    /// </summary>
    [HttpPost("make-admin")]
    [Authorize(Roles = "Heisenberg")]
    public async Task<ActionResult> MakeAdmin(ChangeUserRoleModel requestModel)
    {
        var user = await _usersRepository.GetUserAsync(requestModel.UserId);
        
        if (user is null)
        {
            return BadRequest($"User with id {requestModel.UserId} not found");
        }

        user.Role = requestModel.IsAdmin ? UserRoles.Admin : UserRoles.User;
        
        await _usersRepository.UpdateAsync(user);

        return Ok();
    }
    
    /// <summary>
    /// Get self user info (authorized only)
    /// </summary>
    [Authorize]
    [HttpGet("self-info")]
    public async Task<ActionResult> GetSelfInfo()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        
        var user = await _usersRepository.GetUserAsync(userId);

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Name,
            user.OrgName,
            user.OrgBranch,
            user.OrgActivity,
            user.Role
        });
    }

    private static UserEntity CreateUser(RegisterModel registerModel, IPasswordHasher hasher)
    {
        var hashedPassword = hasher.Hash(registerModel.Password);

        return new UserEntity 
        {
            Login = registerModel.Login,
            Name = registerModel.Name,
            Role = UserRoles.User,
            Password = hashedPassword,
            OrgBranch = registerModel.OrgBranch,
            OrgActivity = registerModel.OrgActivity,
            OrgName = registerModel.OrgName
        };
    }
}
