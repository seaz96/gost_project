using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GostStorage.Infrastructure.Repositories
{
    public class UsersSessionsRepository(DataContext context) : IUserSessionsRepository
    {
        private readonly DataContext _context = context;

        public async Task EraseUserSessionsAsync(long userId)
        {
            await _context.UserSessions.Where(us => us.UserId == userId).ExecuteDeleteAsync();
        }

        public IEnumerable<string?> GetUserSessions(long userId)
        {
            return _context.UserSessions.Where(us => us.UserId == userId).Select(us => us.SessionId).AsEnumerable();
        }

        public async Task RegisterSessionAsync(long userId, string sessionId)
        {
            await _context.UserSessions.AddAsync(new UserSession { UserId = userId, SessionId = sessionId });
        }
    }
}
