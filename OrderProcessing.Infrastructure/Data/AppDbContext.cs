using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Models;

namespace OrderProcessing.Infrastructure.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<Job> Jobs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>()
				.HasMany<Job>()
				.WithOne()
				.HasForeignKey(j => j.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
