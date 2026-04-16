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
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.HasMany(c => c.Addresses)
				.WithOne(a => a.Customer)
				.HasForeignKey(a => a.CustomerId)
				.OnDelete(DeleteBehavior.Cascade);


			builder.HasOne(c => c.DefaultShippingAddress)
				.WithMany()
				.HasForeignKey(c=>c.DefaultShippingAddressId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.HasOne(c=>c.DefaultBillingAddress)
				.WithMany()
				.HasForeignKey(c=>c.DefaultBillingAddressId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(a => a.CreatedAt)
			.IsRequired();

			builder.Property(a => a.UpdatedAt)
			.IsRequired();
		}
	}
}
