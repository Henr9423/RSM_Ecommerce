using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IOrderService
    {
       
        public Task<PlaceOrderResponseDTO> AddCartToOrderAsync(PlaceOrderDTO dto, CancellationToken cancellationToken=default);

        public Task<List<OrderDTO>> GetAllUserOrderDTOSAsync(string userId);

        public Task<OrderDTO> GetUserOrderDTOAsync(int orderId, string userId);
      
        public Task<Order> GetOrderWithOrderNumber(string orderNumber);

        public OrderDTO MapToOrderDTO(Order order);



    }
}
