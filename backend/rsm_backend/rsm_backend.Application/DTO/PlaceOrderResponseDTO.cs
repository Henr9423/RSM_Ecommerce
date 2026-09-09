using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public  class PlaceOrderResponseDTO
    {
        public int OrderId { get; init; }
        public string? GuestAccessToken { get; init; }
        public decimal Total { get; init; }

    }
}
