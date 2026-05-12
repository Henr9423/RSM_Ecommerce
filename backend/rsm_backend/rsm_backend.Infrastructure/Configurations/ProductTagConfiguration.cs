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
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.HasKey(pt => new { pt.ProductID, pt.TagId });
            builder.HasOne(pt=>pt.Tag).WithMany(t => t.ProductTags).HasForeignKey(pt => pt.TagId);
            builder.Property(a => a.CreatedAt)
           .IsRequired();


        }
    }
}
