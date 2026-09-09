using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure
{
    public interface IEmailService
    {
    
        Task SendVerificationCodeAsync(string email, string code);

        Task SendOrderConfirmationAsync(string email, string orderNumber);
        
    }
}
