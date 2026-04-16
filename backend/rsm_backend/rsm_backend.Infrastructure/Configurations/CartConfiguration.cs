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
	public class CartConfiguration : IEntityTypeConfiguration<Cart>
	{
		public void Configure(EntityTypeBuilder<Cart> builder)
		{
			builder.HasKey(ca => ca.Id);

			builder.HasOne(ca=>ca.Customer).WithMany(cu=>cu.Carts).HasForeignKey(ca=>ca.CustomerId).OnDelete(DeleteBehavior.SetNull);

			builder.Property(ca=>ca.Status).IsRequired();

			builder.Property(ca=>ca.CreatedAt).IsRequired();
			builder.Property(ca=>ca.UpdatedAt).IsRequired();

			
		}
	}
}
