using Microsoft.EntityFrameworkCore;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Data
{
	
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{

		}

		public DbSet<Brand> Brands { get; set; }

		public DbSet<Cart> Carts { get; set; }

		public DbSet<CartItem> CartItems { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Customer> Customers { get; set; }
		
		public DbSet<CustomerAddress> CustomerAddresses { get; set; }

		public DbSet<Inventory> Inventories{ get; set; }


		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderAddress> OrderAddresses { get; set; }

		public DbSet<OrderItem> OrderItems { get; set; }

		public DbSet<Payment> Payments { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<ProductCategory> ProductCategories { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }

		public DbSet<ProductVariant> ProductVariants { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}
	}
	
}
