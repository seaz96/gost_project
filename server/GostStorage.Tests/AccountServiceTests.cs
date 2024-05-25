using FluentAssertions;
using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Services.Models.Accounts;
using GostStorage.Services.Services.Abstract;
using GostStorage.Services.Services.Concrete;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GostStorage.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private IUsersRepository _usersRepository;
        private IPasswordHasher _passwordHasher;
        private IUserSessionsRepository _usersSessionsRepository;

        private AccountService _accountService;

        [SetUp]
        public void SetUp()
        {
            _usersRepository = Substitute.For<IUsersRepository>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _usersSessionsRepository = Substitute.For<IUserSessionsRepository>();

            _accountService = new AccountService(_usersRepository, _passwordHasher, _usersSessionsRepository);
        }

        [Test]
        public void Should_not_register_account_with_already_registered_email()
        {
            var login = "test@test.test";

            _usersRepository.IsLoginExistAsync(login).Returns(true);

            var model = new RegisterModel
            {
                Login = login,
                Password = "test",
                Name = "test",
                OrgActivity = "test",
                OrgBranch = "test",
                OrgName = "test",
            };

            _accountService.RegisterAsync(model).GetAwaiter().GetResult();

            _usersRepository.DidNotReceive().AddAsync(Arg.Any<UserEntity>());
        }

        [Test]
        public void Should_register_account_with_not_registered_email()
        {
            var login = "test@test.test";

            _usersRepository.IsLoginExistAsync(login + "0").Returns(true);

            var model = new RegisterModel
            {
                Login = login,
                Password = "test",
                Name = "test",
                OrgActivity = "test",
                OrgBranch = "test",
                OrgName = "test",
            };

            _accountService.RegisterAsync(model).GetAwaiter().GetResult();

            _usersRepository.Received(1).AddAsync(Arg.Any<UserEntity>());
        }

        [Test]
        public void Should_register_user_session_on_registration()
        {
            var model = new RegisterModel
            {
                Login = "test@test.test",
                Password = "test",
                Name = "test",
                OrgActivity = "test",
                OrgBranch = "test",
                OrgName = "test",
            };

            _accountService.RegisterAsync(model).GetAwaiter().GetResult();

            _usersSessionsRepository.Received(1).RegisterSessionAsync(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public void Should_register_user_session_on_login()
        {
            _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            _usersRepository.GetUserAsync(Arg.Any<string>()).Returns(new UserEntity());

            var model = new LoginModel
            {
                Login = "test@test.test",
                Password = "test",
            };

            _accountService.LoginAsync(model).GetAwaiter().GetResult();

            _usersSessionsRepository.Received(1).RegisterSessionAsync(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public void Should_not_login_with_wrong_password()
        {
            _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            var model = new LoginModel
            {
                Login = "test@test.test",
                Password = "test",
            };

            _accountService.LoginAsync(model).GetAwaiter().GetResult();

            _usersSessionsRepository.DidNotReceive().RegisterSessionAsync(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public void Should_login_with_correct_password()
        {
            _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var model = new LoginModel
            {
                Login = "test@test.test",
                Password = "test",
            };

            _accountService.LoginAsync(model).GetAwaiter().GetResult();

            _usersSessionsRepository.DidNotReceive().RegisterSessionAsync(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public void Should_not_change_password_with_wrong_old_password()
        {
            _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            var model = new PasswordChangeModel
            {
                Login = "test@test.test",
                NewPassword = "test",
                OldPassword = "test123",
            };

            _accountService.ChangePasswordAsync(model).GetAwaiter().GetResult();

            _usersRepository.DidNotReceive().UpdatePasswordAsync(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public void Should_change_password_with_correct_old_password()
        {
            _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var model = new PasswordChangeModel
            {
                Login = "test@test.test",
                NewPassword = "test",
                OldPassword = "test123",
            };

            _accountService.ChangePasswordAsync(model).GetAwaiter().GetResult();

            _usersRepository.DidNotReceive().UpdatePasswordAsync(Arg.Any<long>(), Arg.Any<string>());
        }
    }
}
