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
