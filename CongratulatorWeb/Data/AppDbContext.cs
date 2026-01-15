using CongratulatorWeb.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CongratulatorWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
            { }
        public DbSet<Person> People {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Name)
                .IsRequired();                      

                entity.Property(e => e.BirthDate)
                .HasColumnType("date");

                entity.Property(e => e.PhotoPath)
                .HasConversion<string>();
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder
                .Properties<Enum>()
                .HaveConversion<string>();
        }
    }
}
