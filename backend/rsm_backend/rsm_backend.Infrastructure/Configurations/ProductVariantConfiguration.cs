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
	public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
	{
		public void Configure(EntityTypeBuilder<ProductVariant> builder)
		{
			builder.HasKey(pv => pv.Id);

			builder.Property(pv => pv.Id).IsRequired();

			builder.Property(pv => pv.ProductId).IsRequired();

			builder.Property(pv => pv.Sku).IsRequired();

			builder.Property(pv => pv.Price).IsRequired();

			builder.Property(pv => pv.CreatedAt).IsRequired();
			builder.Property(pv => pv.UpdatedAt).IsRequired();



			builder.HasOne(pv => pv.Product)
				.WithMany(p => p.ProductVariants)
				.HasForeignKey(pv => pv.ProductId)
				.OnDelete(DeleteBehavior.Cascade);


			builder.Property(pv => pv.Price)
		   .HasColumnType("decimal(18,2)");

			builder.HasIndex(pv => pv.Sku)
		   .IsUnique();


			builder.ToTable(t =>
			{
				t.HasCheckConstraint(
					"CK_ProductVariant_Price_NonNegative",
					"\"price\" >= 0"
				);
			});
		}
	}
}
