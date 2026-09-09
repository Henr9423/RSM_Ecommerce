using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure
{
    public interface IGuestOrderTokenService
    {
        public string GenerateToken(string orderNumber);
    }
}
