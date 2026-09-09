using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public enum DiscountStatus
    {
        Success,
        InvalidCode,
        Inactive,
        Expired,
        NotStarted,
        MaxUsesReached
    }

    public class DiscountResultDTO
    {

        public bool IsSuccess => Status == DiscountStatus.Success;

        public DiscountStatus Status { get; init; }

        public decimal Amount { get; init; }

        public string? Message { get; init; }
    }
}
