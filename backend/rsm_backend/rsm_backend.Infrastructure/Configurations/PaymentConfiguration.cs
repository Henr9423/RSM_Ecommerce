using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Configurations
{
	public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{
			builder.HasKey(p => p.Id);

			builder.HasOne(p => p.Order).WithMany(o => o.Payments).HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.Restrict);

			

			builder.HasIndex(p => p.StripePaymentIntentId).IsUnique();

			builder.HasIndex(p => p.StripeCheckOutSessionId).IsUnique();

			builder.Property(p=>p.Amount).IsRequired();
			builder.Property(p => p.Amount)
							.HasColumnType("decimal(18,2)")
							.IsRequired();

			builder.Property(p=>p.Currency).IsRequired();

			builder.Property(p => p.Status).IsRequired();

			builder.ToTable(t =>
			{
				t.HasCheckConstraint("ck_check_valid_currency_input_length", "char_length(currency)=3 AND currency=upper(currency)");

				t.HasCheckConstraint(
					"ck_payment_amount_positive",
					"amount > 0"

				);


			});


			builder.Property(a => a.CreatedAt)
			.IsRequired();

			builder.Property(a => a.UpdatedAt)
			.IsRequired();
		}
	}
}
