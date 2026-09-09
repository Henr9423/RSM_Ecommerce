using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using Resend;

namespace rsm_backend.Infrastructure
{
    public class ResendEmailService : IEmailService
    {
        private readonly IResend _resend;
        
        public ResendEmailService(IResend resend)
        {
             _resend= resend;
        }

        public async Task SendOrderConfirmationAsync(string email, string orderNumber)
        {
            var now = DateTime.UtcNow;
            await _resend.EmailSendAsync(new EmailMessage
            {
                From = "RSMBand <onboarding@resend.dev>",
                To = email,
                Subject = $"Order Confirmation for the {now.Date}",
                HtmlBody = $"Your orderNumber is <strong>{orderNumber}</strong>."
            });
        }

        public async Task SendVerificationCodeAsync(string email, string code)
        {
            await _resend.EmailSendAsync(new EmailMessage
            {
                From = "RSMBand <onboarding@resend.dev>",
                To = email,
                Subject = "Your verification code",
                HtmlBody = $"Your verification code is <strong>{code}</strong>."
            });
        }
    }
}
