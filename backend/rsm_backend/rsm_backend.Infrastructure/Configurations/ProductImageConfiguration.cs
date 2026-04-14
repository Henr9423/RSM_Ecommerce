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
	public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
	{
		public void Configure(EntityTypeBuilder<ProductImage> builder)
		{
			builder.HasKey(pi => pi.Id);

			builder.HasOne(pi => pi.ProductVariant)
				.WithMany(pv => pv.ProductImages)
				.HasForeignKey(pi => pi.ProductVariantId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);
			
			builder.Property(pi => pi.StorageKey)
				.IsRequired();

			builder.Property(pi => pi.IsPrimary)
				.IsRequired();

			builder.Property(pi => pi.SortOrder).IsRequired();

			builder.Property(pi => pi.CreatedAt).IsRequired();

			builder.Property(pi => pi.UpdatedAt).IsRequired();

		}
	}
}
