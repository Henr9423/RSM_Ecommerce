using Microsoft.EntityFrameworkCore;
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
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly AppDbContext _context;

        public DiscountCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBulkDiscountCodesAsync(List<DiscountCode> discountCodes)
        {
           await _context.AddRangeAsync(discountCodes);

            await _context.SaveChangesAsync();
        }

        public async Task AddDiscountCodeAsync(DiscountCode discount)
        {
            _context.Add(discount);

            await _context.SaveChangesAsync();
        }

        public async Task<DiscountCode?> GetDiscountAsync(string code)
        {

            var discount = await _context.DiscountCodes.FirstOrDefaultAsync(x => x.Code == code);

           return discount;
        }
    }
}
