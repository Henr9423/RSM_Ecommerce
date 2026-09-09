using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories
{
    public interface IDiscountCodeRepository
    {
        Task<DiscountCode?> GetDiscountAsync(string code);

        Task AddDiscountCodeAsync(DiscountCode discount);

        Task AddBulkDiscountCodesAsync(List<DiscountCode> discountCodes);

    }
}
