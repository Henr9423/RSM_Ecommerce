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
	public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.HasKey(oi => oi.Id);

			builder.HasOne(oi=>oi.Order)
				.WithMany(o=>o.OrderItems)
				.HasForeignKey(oi=>oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(oi => oi.ProductVariant)
				.WithMany(pv => pv.OrderItems)
				.HasForeignKey(oi => oi.ProductVariantId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(oi => oi.Quantity).IsRequired();
			builder.Property(oi => oi.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
			builder.Property(oi => oi.Discount).HasColumnType("decimal(10,2)").IsRequired();
			builder.Property(oi => oi.LineTotal).HasColumnType("decimal(10,2)").IsRequired();
			builder.Property(oi => oi.CreatedAt).IsRequired();

			builder.ToTable(t =>
			{
				t.HasCheckConstraint("ck_order_item_quantity_positive", "quantity > 0");
				t.HasCheckConstraint("ck_order_item_unit_price_non_negative", "unit_price >= 0");
				t.HasCheckConstraint("ck_order_item_discount_non_negative", "discount >= 0");
				t.HasCheckConstraint("ck_order_item_discount_valid", "discount <= unit_price * quantity");
				t.HasCheckConstraint("ck_order_item_line_total_non_negative", "line_total >= 0");
				t.HasCheckConstraint("ck_order_item_line_total_valid", "line_total = (unit_price * quantity) - discount");
			});

		}
	}
}
