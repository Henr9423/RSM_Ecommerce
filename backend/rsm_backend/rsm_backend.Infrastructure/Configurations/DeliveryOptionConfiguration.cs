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
    public class DeliveryOptionConfiguration : IEntityTypeConfiguration<DeliveryOption>
    {
        public void Configure(EntityTypeBuilder<DeliveryOption> builder)
        {
            builder.HasKey(d => d.Id);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("ck_delivery_options_price_positive_or_zero", "price>=0");
                t.HasCheckConstraint("ck_delivery_options_min_delivery_days_positive_or_zero", "min_delivery_days>=0");
                t.HasCheckConstraint("ck_delivery_options_max_delivery_days_positive_or_zero", "max_delivery_days>=0");
            });

            builder.Property(d => d.Price).IsRequired();

            builder.Property(d => d.Price)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(d=>d.MinDeliveryDays).IsRequired();

            builder.Property(d => d.MaxDeliveryDays).IsRequired();
        }
    }
}
