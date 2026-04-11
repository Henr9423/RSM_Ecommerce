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
	public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
	{
		public void Configure(EntityTypeBuilder<CustomerAddress> builder)
		{
			
			builder.HasKey(p => p.Id);

			builder.Property(p => p.FullName).IsRequired().HasMaxLength(100);

			builder.Property(a => a.AddressLine1)
			   .IsRequired()
			   .HasMaxLength(200);

			builder.Property(a => a.AddressLine2)
				.HasMaxLength(200);

			builder.Property(a => a.City)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(a => a.StateOrRegion)
				.HasMaxLength(100);

			builder.Property(a => a.PostalCode)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(a => a.Country)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(a => a.PhoneNumber)
				.HasMaxLength(50);

			builder.Property(a => a.CreatedAt)
				.IsRequired();

			builder.Property(a => a.IsDefault)
				.IsRequired();

			builder.HasIndex(a => a.CustomerId);
		}
	}
}
