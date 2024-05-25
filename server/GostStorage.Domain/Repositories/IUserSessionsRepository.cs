namespace GostStorage.Domain.Repositories
{
    public interface IUserSessionsRepository
    {
        public Task<bool> IsSessionRegistered(long userId, string sessionId);

        public Task RegisterSessionAsync(long userId, string sessionId);

        public Task EraseUserSessionsAsync(long userId);
    }
}
