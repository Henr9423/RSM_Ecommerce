using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests.Support
{
    public class AcceptanceScenarioContext : IDisposable
    {

        public int CustomerId { get; set; }
        public string Email { get; set; } = null!;
        public string OrderNumber { get; set; } = null!;
        public string? VerificationCode { get; set; }

        public string GuestCartToken { get; set; } = null!;

        public HttpClient Client { get; set; } = null!;

        public HttpResponseMessage? Response { get; set; }
        public void Dispose()
        {
            Response?.Dispose();
            Client?.Dispose();
        }
    }
}
