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

		public DbSet<OrderAddress> OrderAddresses { get; set; }
		public DbSet<CustomerAddress> CustomerAddresses { get; set; }
		public DbSet<Customer> Customers { get; set; }
		
		public  DbSet<Order> Orders { get; set; }

		public DbSet<Brand> Brands { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}
	}
	
}
