using System.Security.Claims;
using GostStorage.API.Entities;
using GostStorage.API.Helpers;
using GostStorage.API.Models.Accounts;
using GostStorage.API.Navigations;
using GostStorage.API.Repositories.Interfaces;
using GostStorage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.API.Services.Concrete;

public class AccountService(IUsersRepository usersRepository, IPasswordHasher passwordHasher) : IAccountService
{
    public async Task<IActionResult> LoginAsync(LoginModel loginModel)
    {
        var user = await usersRepository.GetUserAsync(loginModel.Login);

        if (user is null)
        {
            return new BadRequestObjectResult(new { Field = nameof(loginModel.Login) });
        }

        var verified = passwordHasher.Verify(loginModel.Password, user.Password!);

        if (!verified)
        {
            return new BadRequestObjectResult(new { Field = nameof(loginModel.Password) });
        }

        var token = SecurityHelper.GetAuthToken(user);

        return new OkObjectResult(new
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
    
    public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
    {
        var isLoginExist = await usersRepository.IsLoginExistAsync(registerModel.Login);

        if (isLoginExist)
        {
            return new ConflictObjectResult(new { Field = nameof(registerModel.Login) });
        }

        var user = CreateUser(registerModel, passwordHasher);

        var task = usersRepository.AddAsync(user);

        var token = SecurityHelper.GetAuthToken(user);

        task.Wait();

        return new OkObjectResult(new
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
    
    public async Task<IActionResult> RestorePasswordAsync(PasswordRestoreModel passwordRestoreModel)
    {
        var user = await usersRepository.GetUserAsync(passwordRestoreModel.Login);

        if (user is null)
        {
            return new BadRequestObjectResult(new { Field = nameof(passwordRestoreModel.Login) });
        }

        await usersRepository.UpdatePasswordAsync(user.Id, passwordHasher.Hash(passwordRestoreModel.NewPassword));

        return new OkResult();
    }
    
    public async Task<IActionResult> ChangePasswordAsync(PasswordChangeModel passwordChangeModel)
    {
        var user = await usersRepository.GetUserAsync(passwordChangeModel.Login);
        
        if (user is null)
        {
            return new BadRequestObjectResult(new { Field = nameof(passwordChangeModel.Login) });
        }

        if (!passwordHasher.Verify(passwordChangeModel.OldPassword, user.Password!))
        {
            return new BadRequestObjectResult("Old password is wrong");
        }
        
        await usersRepository.UpdatePasswordAsync(user.Id, passwordHasher.Hash(passwordChangeModel.NewPassword));

        return new OkResult();
    }

    public async Task<IActionResult> GetUsersListAsync()
    {
        var users = (await usersRepository.GetAllAsync()).Select(user => new 
        {
            user.Id,
            user.Name,
            user.Login,
            Role = user.Role.ToString(),
        }).OrderBy(x => x.Id);

        return new OkObjectResult(users);
    }
    
    public async Task<IActionResult> GetUserInfoAsync(long id)
    {
        var user = await usersRepository.GetUserAsync(id);
        
        if (user is null)
        {
            return new BadRequestObjectResult($"No user with such id {id}");
        }

        return new OkObjectResult(new
        {
            user.Id,
            user.Login,
            user.Name,
            user.OrgBranch,
            user.OrgActivity,
            user.OrgName,
            Role = user.Role.ToString()
        });
    }
    
    public async Task<IActionResult> SelfEditAsync(UserSelfEditModel userSelfEditModel, long id)
    {
        var user = await usersRepository.GetUserAsync(id);

        if (user is null)
        {
            return new BadRequestResult();
        }
        
        user.Name = userSelfEditModel.Name ?? user.Name;
        user.OrgName = userSelfEditModel.OrgName ?? user.OrgName;
        user.OrgBranch = userSelfEditModel.OrgBranch ?? user.OrgBranch;
        user.OrgActivity = userSelfEditModel.OrgActivity ?? user.OrgActivity;

        await usersRepository.UpdateAsync(user);

        return new OkResult();
    }
    
    public async Task<IActionResult> AdminEditAsync(UserAdminEditModel userAdminEditModel, ClaimsPrincipal userPrincipal)
    {
        var user = await usersRepository.GetUserAsync(userAdminEditModel.Login);

        if (user is null)
        {
            return new BadRequestObjectResult(new { Field = nameof(userAdminEditModel.Login) });
        }
        
        if (user.Role != UserRoles.User && userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value != "Heisenberg")
        {
            return new BadRequestObjectResult("You don't have permission");
        }
        
        user.Name = userAdminEditModel.Name ?? user.Name;
        user.OrgName = userAdminEditModel.OrgName ?? user.OrgName;
        user.OrgBranch = userAdminEditModel.OrgBranch ?? user.OrgBranch;
        user.OrgActivity = userAdminEditModel.OrgActivity ?? user.OrgActivity;
        
        await usersRepository.UpdateAsync(user);

        return new OkResult();
    }

    public async Task<IActionResult> MakeAdminAsync(ChangeUserRoleModel requestModel)
    {
        var user = await usersRepository.GetUserAsync(requestModel.UserId);
        
        if (user is null)
        {
            return new BadRequestObjectResult($"User with id {requestModel.UserId} not found");
        }

        user.Role = requestModel.IsAdmin ? UserRoles.Admin : UserRoles.User;
        
        await usersRepository.UpdateAsync(user);

        return new OkResult();
    }
    
    public async Task<IActionResult> GetSelfInfoAsync(long userId)
    {
        var user = await usersRepository.GetUserAsync(userId);

        return new OkObjectResult(new
        {
            user.Id,
            user.Login,
            user.Name,
            user.OrgName,
            user.OrgBranch,
            user.OrgActivity,
            Role = user.Role.ToString()
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