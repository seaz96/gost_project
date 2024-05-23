using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GostStorage.Domain.Repositories
{
    public interface IUserSessionsRepository
    {
        public IEnumerable<string?> GetUserSessions(long userId);

        public Task RegisterSessionAsync(long userId, string sessionId);

        public Task EraseUserSessionsAsync(long userId);
    }
}
