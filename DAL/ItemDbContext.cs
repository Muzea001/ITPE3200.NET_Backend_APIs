using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oblig1.Models;

namespace Oblig1.DAL
{
    public class ItemDbContext : IdentityDbContext<Person>

    {

        public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
        {


        }
        public DbSet<Ordre> Ordre { get; set; }
        public DbSet<Hus> Hus { get; set; } 
        public DbSet<Kunde> Kunde { get; set; } 

        public DbSet <Person> Person { get; set; }

        public DbSet<Eier> Eier { get; set; }

        public DbSet<Bilder> Bilder { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>()
             .ToTable("Person");
            modelBuilder.Entity<IdentityUserLogin<string>>()
           .HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<Kunde>().ToTable("Kunde");
            modelBuilder.Entity<Eier>().ToTable("Eier");
            modelBuilder.Entity<Kunde>()
            .Ignore(c => c.husListe);

            modelBuilder.Entity<Kunde>()
            .Ignore(c => c.ordreListe);

           
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }

    
}
