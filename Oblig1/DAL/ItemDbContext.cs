using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oblig1.Models;

namespace Oblig1.DAL
{
    public class ItemDbContext : IdentityDbContext

    {
        public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
        {


        }
        public DbSet<Person> person { get; set; }
        public DbSet<Hus> hus { get; set; } 
        public DbSet<Kunde> kunde { get; set; } 

        public DbSet <Ordre> ordre { get; set; }

        public DbSet<Eier> eier { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

         


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<Kunde>().ToTable("kunde");
            modelBuilder.Entity<Eier>().ToTable("eier");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }

    
}
