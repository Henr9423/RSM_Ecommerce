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
	public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
	{
		public void Configure(EntityTypeBuilder<Inventory> builder)
		{
			builder.HasKey(i => i.ProductVariantId);

			builder.HasOne(i => i.ProductVariant)
				.WithOne(pv => pv.Inventory)
				.HasForeignKey<Inventory>(i => i.ProductVariantId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(i => i.QuantityInStock).IsRequired();


			builder.Property(i => i.ReservedQuantity).IsRequired();

			builder.Property(i => i.CreatedAt).IsRequired();

			builder.Property(i => i.UpdatedAt).IsRequired();


			builder.ToTable(t =>
			{
				t.HasCheckConstraint("ck_inventory_quantity_in_stock_non_negative", "quantity_in_stock >= 0");
				t.HasCheckConstraint("ck_inventory_reserved_quantity_non_negative", "reserved_quantity >= 0");
				t.HasCheckConstraint("ck_inventory_reserved_lte_stock", "reserved_quantity <= quantity_in_stock");
			});
		}
	}
}
