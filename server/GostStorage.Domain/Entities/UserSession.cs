using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GostStorage.Domain.Entities
{
    public class UserSession
    {
        public long UserId { get; set; }

        public string? SessionId { get; set; }
    }
}
