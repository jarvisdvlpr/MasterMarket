using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewMasterMarket.Models;

namespace NewMasterMarket.DAL
{
    public class AppDbContext: IdentityDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Item>().Property(x=>x.MinAmount).IsRequired().HasDefaultValue(1);
            base.OnModelCreating(builder);
        }


    }
}
