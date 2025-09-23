using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using OneInteg.Server.Domain.Entities;

namespace OneInteg.Server.DataAccess
{
    public class OneIntegDbContext : DbContext
    {
        public OneIntegDbContext(DbContextOptions<OneIntegDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenant { get; init; }
        public DbSet<Customer> Customer { get; init; }
        public DbSet<Subscription> Subscription { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().ToCollection("tenant");
            modelBuilder.Entity<Customer>().ToCollection("customer");
            modelBuilder.Entity<Subscription>().ToCollection("subscription");
        }
    }
}
