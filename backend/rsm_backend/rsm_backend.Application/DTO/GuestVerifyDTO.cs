using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class GuestVerifyDTO
    {
        public string OrderNumber { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }
    
}
