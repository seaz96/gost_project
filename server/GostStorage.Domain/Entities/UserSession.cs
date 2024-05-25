using GostStorage.Domain.Entities.Base;

namespace GostStorage.Domain.Entities
{
    public class UserSession : BaseEntity
    {
        public long UserId { get; set; }

        public string? SessionId { get; set; }
    }
}
