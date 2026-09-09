using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IGuestOrderAccessService
    {
        public Task RequestAccessAsync(string orderNumber, string email, CancellationToken cancellationToken);

        public Task<GuestVerificationResult> VerifyGuestAsync(string orderNumber, string code, CancellationToken cancellationToken);
    }
}
