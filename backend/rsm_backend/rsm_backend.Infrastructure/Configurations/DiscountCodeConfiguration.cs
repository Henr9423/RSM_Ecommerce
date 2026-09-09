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
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.HasKey(x=> x.Id);
            builder.Property(x=>x.IsActive).IsRequired();
            builder.Property(x=>x.Type).IsRequired();
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.StartsAt).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();

            
        }
    }
}
