using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Infrastructure.Repositories
{
    public class UsersSessionsRepository(DataContext context) : IUserSessionsRepository
    {
        private readonly DataContext _context = context;

        public async Task EraseUserSessionsAsync(long userId)
        {
            await _context.UserSessions.Where(us => us.UserId == userId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSessionRegistered(long userId, string sessionId)
        {
            return await _context.UserSessions.AnyAsync(us => us.UserId == userId && us.SessionId == sessionId);
        }

        public async Task RegisterSessionAsync(long userId, string sessionId)
        {
            await _context.UserSessions.AddAsync(new UserSession { UserId = userId, SessionId = sessionId });
            await _context.SaveChangesAsync();
        }
    }
}
