using rsm_backend.Application.Services.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.FakeServicesForTests
{
 
    public class FakeEmailService : IEmailService
    {
        public List<SentEmail> SentEmails { get;}= new List<SentEmail>();
        public Task SendOrderConfirmationAsync(string email, string orderNumber)
        {
            SentEmails.Add(new SentEmail(email, "Order Confirmation" , orderNumber));

            return Task.CompletedTask;
        }

        public Task SendVerificationCodeAsync(string email, string code)
        {
            SentEmails.Add(new SentEmail(email, "Verification Code" , code));

            return Task.CompletedTask;
        }

        public void Clear()
        {
            SentEmails.Clear();
        }
    }

    public record SentEmail(string To, string subject, string Body);


}
