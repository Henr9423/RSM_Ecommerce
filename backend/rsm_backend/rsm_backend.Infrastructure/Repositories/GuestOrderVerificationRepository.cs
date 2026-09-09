using Microsoft.EntityFrameworkCore;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Repositories
{
    public class GuestOrderVerificationRepository : IGuestOrderVerificationRepository
    {
        private readonly AppDbContext _context;

        public GuestOrderVerificationRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task AddAsync(GuestOrderVerification guestOrderVerification)
        {
            _context.GuestOrderVerifications.Add(guestOrderVerification);
        }

        public async Task<GuestOrderVerification?> GetByOrderId(int orderId)
        {
           return await _context.GuestOrderVerifications.FindAsync(orderId);
        }

        public async Task<GuestOrderVerification?> GetByOrderNumber(string orderNumber)
        {
            return await _context.GuestOrderVerifications.Include(gov => gov.Order).FirstOrDefaultAsync(gov => gov.Order.OrderNumber == orderNumber);
        }
    }
}
