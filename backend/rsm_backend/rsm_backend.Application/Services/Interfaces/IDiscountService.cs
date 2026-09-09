using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IDiscountService
    {
        public Task<DiscountResultDTO> CalculateDiscountAsync(decimal cartTotal, string? couponCode);

        public Task BulkCreate(List<CreateDiscountDTO> createDiscountDTOs);

    }
}
