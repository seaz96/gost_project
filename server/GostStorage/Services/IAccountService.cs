using System.Security.Claims;
using GostStorage.Models.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services;

public interface IAccountService
{
    public Task<IActionResult> LoginAsync(LoginModel loginModel);

    public Task<IActionResult> RegisterAsync(RegisterModel registerModel);

    public Task<IActionResult> RestorePasswordAsync(PasswordRestoreModel passwordRestoreModel);

    public Task<IActionResult> ChangePasswordAsync(PasswordChangeModel passwordChangeModel);

    public Task<IActionResult> GetUsersListAsync();

    public Task<IActionResult> GetUserInfoAsync(long id);

    public Task<IActionResult> SelfEditAsync(UserSelfEditModel userSelfEditModel, long id);

    public Task<IActionResult> AdminEditAsync(UserAdminEditModel userAdminEditModel, ClaimsPrincipal userPrincipal);

    public Task<IActionResult> MakeAdminAsync(ChangeUserRoleModel requestModel);

    public Task<IActionResult> GetSelfInfoAsync(long userId);
}