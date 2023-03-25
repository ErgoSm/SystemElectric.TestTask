using Microsoft.EntityFrameworkCore;
using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.MsSQL.Context
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CarEntry> CarEntries { get; set; }
        public DbSet<DriverEntry> DriverEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarEntry>()
                .ToTable("cars")
                .HasKey(x => x.Timestamp);

            modelBuilder.Entity<CarEntry>()
                .Property(x => x.Timestamp)
                .HasColumnName("timestamp")
                .HasColumnType("datetime");

            modelBuilder.Entity<CarEntry>()
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("nvarchar(50)");

            modelBuilder.Entity<DriverEntry>()
                .ToTable("drivers")
                .HasKey(x => x.Timestamp);

            modelBuilder.Entity<DriverEntry>()
                .Property(x => x.Timestamp)
                .HasColumnName("timestamp")
                .HasColumnType("datetime");

            modelBuilder.Entity<DriverEntry>()
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("nvarchar(50)");
        }
    }
}
