using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface IGuestOrderVerificationRepository
    {
       Task AddAsync(GuestOrderVerification guestOrderVerification);

        Task<GuestOrderVerification?> GetByOrderId(int orderId);

        Task<GuestOrderVerification?> GetByOrderNumber(string orderNumber);
    }
}
