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
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p=>p.BrandId).IsRequired();

			builder.Property(p => p.Name).IsRequired();
			builder.Property(p => p.Description).IsRequired();

			builder.HasOne(p => p.Brand)
				.WithMany(b=>b.Products)
				.HasForeignKey(p => p.BrandId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
