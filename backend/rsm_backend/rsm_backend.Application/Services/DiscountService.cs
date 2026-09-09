using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountCodeRepository _discountRepo;

        public DiscountService(IDiscountCodeRepository discountRepo)
        {
            _discountRepo = discountRepo;
        }

        public async Task BulkCreate(List<CreateDiscountDTO> createDiscountDTOs)
        {
            if (createDiscountDTOs == null)
                throw new ArgumentNullException(nameof(createDiscountDTOs));


            if (createDiscountDTOs.Any(b => b == null))
                throw new ArgumentException("CreateDiscountDTOs list contains null items.", nameof(createDiscountDTOs));
            
            var discountCodes=createDiscountDTOs.Select(x => new DiscountCode()
            {

                Code = x.Code,
                ExpiresAt = x.ExpiresAt,
                IsActive = x.IsActive,
                MaxUses = x.MaxUses,
                UsedCount = x.UsedCount,
                MinimumOrderAmount = x.MinimumOrderAmount,
                MaxUsesPerCustomer = x.MaxUsesPerCustomer,
                StartsAt = x.StartsAt,
                Type = x.Type,
                Value = x.Value,

            }).ToList();

            await _discountRepo.AddBulkDiscountCodesAsync(discountCodes);
        }

        public async Task<DiscountResultDTO> CalculateDiscountAsync(decimal cartTotal, string? couponCode)
        {
         

            var now = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(couponCode))
                return new DiscountResultDTO()
                {
                    Status = DiscountStatus.InvalidCode,
                    Amount = 0m,
                    Message="couponCode is null,whiteSpace or empty"
                    
                };


            var discountInfo = await _discountRepo.GetDiscountAsync(couponCode);
           
            if (discountInfo==null)
            {
                return new DiscountResultDTO()
                {
                    Status = DiscountStatus.InvalidCode,
                    Amount = 0m,
                    Message="couponCode is not valid"
                   
                };
            }

            if(!discountInfo.IsActive)
            {
                return new DiscountResultDTO()
                {
                    Status = DiscountStatus.Inactive,
                    Amount = 0m,
                    Message= "Coupoun code is inactive"
                    
                };
            }

            if(discountInfo.UsedCount>=discountInfo.MaxUses)
            {

                return new DiscountResultDTO()
                {
                    Status = DiscountStatus.MaxUsesReached,
                    Amount = 0m,
                    Message="Max uses for coupon code has been reached"

                };
            }
           
            if (discountInfo.StartsAt<=now && now<=discountInfo.ExpiresAt)
            {
                switch (discountInfo.Type)
                {
                    case DiscountType.Percentage:

                        return new DiscountResultDTO()
                        {
                            Status = DiscountStatus.Success,
                            Amount = cartTotal * (discountInfo.Value / 100),
                            Message="Succesfully calculated a percentage discount"

                        };
                     
                 
                    case DiscountType.FixedAmount:
                        return new DiscountResultDTO()
                        {
                            Status = DiscountStatus.Success,
                            Amount = discountInfo.Value,
                            Message = "Succesfully made a fixed amount discount"

                        };

                    default:
                        
                        throw new InvalidOperationException("DiscountType has not been defined for the coupon code");
                        
                }
            }

            return new DiscountResultDTO() { Status=DiscountStatus.Expired, Amount=0m, Message="Discount code is expired"};


        }
    }
}
