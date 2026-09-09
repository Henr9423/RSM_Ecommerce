using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IAdminDeliveryOptionService
    {

        public Task<int> BulkCreate(List<CreateDeliveryOptionDTO> deliveryOptions);


        public Task<List<DeliveryOptionDTO>> GetAllDeliveryOptions();
    }
}
