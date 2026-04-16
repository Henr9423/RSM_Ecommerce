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
	public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
	{
		public void Configure(EntityTypeBuilder<CartItem> builder)
		{
			builder.HasKey(ci => ci.Id);

			builder.HasOne(ci=>ci.Cart).WithMany(ca=>ca.Items).HasForeignKey(ci=>ci.CartId).OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(ci=>ci.ProductVariant).WithMany(pv=>pv.CartItems).HasForeignKey(ci=>ci.ProductVariantId).OnDelete(DeleteBehavior.Restrict);
			
			builder.HasIndex(ci => new { ci.CartId,ci.ProductVariantId }).IsUnique();

			builder.Property(ci => ci.Quantity).IsRequired();

			builder.Property(ci => ci.CartId).IsRequired();
			builder.Property(ci => ci.ProductVariantId).IsRequired();

			builder.ToTable(t =>
			{
				t.HasCheckConstraint("ck_cart_item_quantity_positive", "quantity>0");


			});

		}
	}
}
