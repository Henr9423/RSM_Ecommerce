using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Admin
{
    public class AdminDeliveryOptionService : IAdminDeliveryOptionService
    {

        private readonly IDeliveryOptionRepository _deliveryOptionRepo;

        
        public AdminDeliveryOptionService(IDeliveryOptionRepository deliveryOptionRepo)
        {
            _deliveryOptionRepo = deliveryOptionRepo;
        }

        public async Task<int> BulkCreate(List<CreateDeliveryOptionDTO> deliveryOptions)
        {
            if (deliveryOptions == null)
                throw new ArgumentNullException(nameof(deliveryOptions));

            if (deliveryOptions.Count == 0)
                return 0;

            if (deliveryOptions.Any(b => b == null))
                throw new ArgumentException("DeliveryOptions list contains null items.", nameof(deliveryOptions));

            var entities = deliveryOptions.Select(item => new DeliveryOption
            {
                Name = item.Name,
                MaxDeliveryDays = item.MaxDeliveryDays,
                MinDeliveryDays= item.MinDeliveryDays,
                Price = item.Price,
            }).ToList();

            await _deliveryOptionRepo.BulkCreateAsync(entities);

            await _deliveryOptionRepo.SaveChangesAsync();

            return entities.Count;
        }

        public async Task<List<DeliveryOptionDTO>> GetAllDeliveryOptions()
        {
            List<DeliveryOptionDTO> optionDTOs = new List<DeliveryOptionDTO>();
            List<DeliveryOption> delOptions= await _deliveryOptionRepo.GetAllDeliveryOptionsAsync();
            
            var now = DateTime.UtcNow;

            foreach (var option in delOptions)
            {
                var optionDTO = new DeliveryOptionDTO()
                {
                    Name= option.Name,
                    Id = option.Id,
                    MinDeliveryDays = option.MinDeliveryDays,
                    MaxDeliveryDays = option.MaxDeliveryDays,
                    EstimatedDeliveryFrom= AddBusinessDays(now,option.MinDeliveryDays),
                    EstimatedDeliveryTo=AddBusinessDays(now,option.MaxDeliveryDays),
                    Price = option.Price,
                };

                optionDTOs.Add(optionDTO);
            }

            return optionDTOs;
        }

        private static DateTime AddBusinessDays(DateTime date, int businessDays)
        {
            if (businessDays < 0)
                throw new ArgumentOutOfRangeException(nameof(businessDays));

            var result = date;
            var addedDays = 0;

            while (addedDays < businessDays)
            {
                result = result.AddDays(1);

                if (result.DayOfWeek is not DayOfWeek.Saturday
                    and not DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return result;
        }
    }
}
