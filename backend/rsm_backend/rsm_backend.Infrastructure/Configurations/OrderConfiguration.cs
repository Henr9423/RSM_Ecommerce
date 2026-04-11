using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(o => o.Status)
				.HasConversion<string>()
				.IsRequired();

			builder.Property(p => p.ShippingFee)
				.HasColumnType("decimal(10,2)");

			builder.Property(p => p.Subtotal)
			.HasColumnType("decimal(10,2)");

			builder.Property(p => p.Tax)
			.HasColumnType("decimal(10,2)");

			builder.Property(p => p.Total)
			.HasColumnType("decimal(10,2)");

			builder.HasOne(o=>o.Customer).WithMany(c=> c.Orders).HasForeignKey(o => o.CustomerId);

			builder.HasOne(c => c.ShippingAddress)
				.WithMany()
				.HasForeignKey(c => c.BillingAddressId)
				.OnDelete(DeleteBehavior.Restrict);



			builder.HasOne(c => c.BillingAddress)
				.WithMany()
				.HasForeignKey(c => c.BillingAddressId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.Property(a => a.CreatedAt)
			.IsRequired();

			builder.Property(c => c.CustomerId)
				.IsRequired();


			builder.ToTable(t =>
			{

				t.HasCheckConstraint("ck_order_shipping_fee_non_negative", "shipping_fee >= 0");
				t.HasCheckConstraint("ck_order_subtotal_non_negative", "subtotal >= 0");
				t.HasCheckConstraint("ck_order_tax_non_negative", "tax >= 0");
				t.HasCheckConstraint("ck_order_discount_negative", "discount <= 0");
				t.HasCheckConstraint("ck_order_discount_non_negative", "discount >= 0");

				t.HasCheckConstraint(
					"ck_order_discount_lte_subtotal",
					"discount <= subtotal"
				);

				t.HasCheckConstraint(
					"ck_order_total_valid",
					"total = subtotal + tax + shipping_fee-discount-discount"
				);
			});
		}
	}
}
