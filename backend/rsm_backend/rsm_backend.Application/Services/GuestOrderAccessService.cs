using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services
{
    public enum GuestVerificationResult
    {
        Success,
        InvalidCode,
        Expired,
        TooManyAttempts,
        NotFound
    }
    public class GuestOrderAccessService : IGuestOrderAccessService
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly IGuestOrderVerificationRepository _guestOrderVerificationRepo;
        private readonly IUnitOfWork _unitOfWork;
        public GuestOrderAccessService(IOrderService orderService, IEmailService emailService, IGuestOrderVerificationRepository guestOrderVerificationRepository, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _emailService = emailService;
            _guestOrderVerificationRepo = guestOrderVerificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RequestAccessAsync(string orderNumber, string email, CancellationToken cancellationToken)
        {
           var order= await _orderService.GetOrderWithOrderNumber(orderNumber);

            if (order is null || !order.Customer.IsGuest || !string.Equals(order.Customer.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var now = DateTime.UtcNow;

            var code = RandomNumberGenerator
                        .GetInt32(100000, 1000000)
                        .ToString();

            var verification = await _guestOrderVerificationRepo.GetByOrderId(order.Id);

            if(verification is null)
            {
               verification= new GuestOrderVerification()
                {
                    OrderId = order.Id
                };

                await _guestOrderVerificationRepo.AddAsync(verification);


            }

            verification.CodeHash = HashToken(code);
            verification.AttemptCount = 0;
            verification.CreatedAt = now;
            verification.ExpiresAt = now.AddMinutes(15);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendVerificationCodeAsync(email, code);
        }
        


        private static string HashToken(string token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        public async Task<GuestVerificationResult> VerifyGuestAsync(string orderNumber, string code, CancellationToken cancellationToken)
        {
          var verification= await _guestOrderVerificationRepo.GetByOrderNumber(orderNumber);
          var now= DateTime.UtcNow;
            
           
            if (verification is null)  
            {
                return GuestVerificationResult.NotFound;
            }

            if (verification.AttemptCount >= 3)
            {
                return GuestVerificationResult.TooManyAttempts;
            }


            if (verification.CodeHash != HashToken(code))
            {
                verification.AttemptCount += 1;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return GuestVerificationResult.InvalidCode;
            }

            if(verification.ExpiresAt < now)
            {
                return GuestVerificationResult.Expired;
            }

           
            

             return GuestVerificationResult.Success;
        }
    }
}
