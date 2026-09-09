using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Configurations
{
    public class GuestOrderVerificationConfiguration : IEntityTypeConfiguration<GuestOrderVerification>
    {
        public void Configure(EntityTypeBuilder<GuestOrderVerification> builder)
        { 

            builder.HasOne(gov => gov.Order)
            .WithOne(o => o.GuestOrderVerification)
            .HasForeignKey<GuestOrderVerification>(gov => gov.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
