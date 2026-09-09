using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
    public class GuestOrderVerification
    {
       
            public int Id { get; set; }

            public int OrderId { get; set; }
            public Order Order { get; set; } = null!;

            public string CodeHash { get; set; } = null!;

            public DateTime ExpiresAt { get; set; }

            public DateTime? UsedAt { get; set; }

            public int AttemptCount { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    
}
