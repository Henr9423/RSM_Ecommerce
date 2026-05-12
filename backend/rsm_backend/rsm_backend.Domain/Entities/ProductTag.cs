using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Domain.Entities
{
    public class ProductTag
    {
        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; }=null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
