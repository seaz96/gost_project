using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GostStorage.Domain.Entities.Base;

namespace GostStorage.Domain.Entities
{
    public class UserSession : BaseEntity
    {
        public long UserId { get; set; }

        public string? SessionId { get; set; }
    }
}
