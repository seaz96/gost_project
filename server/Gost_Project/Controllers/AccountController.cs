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

namespace Gost_Project.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController(IPasswordHasher passwordHasher, IUsersRepository usersRepository) : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        private readonly IUsersRepository _usersRepository = usersRepository;

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

            return Ok();
        }

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

            return Ok();
        }

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

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] PasswordChangeModel passwordChangeModel)
        {
            var user = await _usersRepository.GetUserAsync(passwordChangeModel.Login);

            if (user is null)
            {
                return BadRequest(new { Field = nameof(passwordChangeModel.Login) });
            }

            user.Password = _passwordHasher.Hash(passwordChangeModel.NewPassword);

            return Ok();
        }

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

        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([FromBody] UserEditModel userEditModel)
        {
            var user = await _usersRepository.GetUserAsync(userEditModel.Login);

            if (user is null)
            {
                return BadRequest(new { Field = nameof(userEditModel.Login) });
            }

            

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
                Password = hashedPassword 
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
}
