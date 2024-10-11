using Microsoft.EntityFrameworkCore;
using SimpleCustomer.WebAPI.Data.Models;

namespace SimpleCustomer.WebAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext() : base() { }
        public ApplicationDbContext(DbContextOptions options): base(options) { }

        public DbSet<Customer> customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        }
    }
}
