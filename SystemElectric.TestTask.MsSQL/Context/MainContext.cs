﻿using Microsoft.EntityFrameworkCore;
using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.MsSQL.Context
{
    public class MainContext : DbContext
    {
        private Dictionary<string, string> _typesMapping;
        private Action? _configure;

        public MainContext(DbContextOptions<MainContext> options, Dictionary<string, string> typesMapping, Action? configure = null) : base(options)
        {
            _typesMapping = typesMapping;
            _configure = configure;
            Database.EnsureCreated();
        }

        public DbSet<CarEntry> CarEntries { get; set; }
        public DbSet<DriverEntry> DriverEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _configure?.Invoke();

            modelBuilder.Entity<CarEntry>()
                .ToTable("cars")
                .HasKey(x => x.Timestamp);

            modelBuilder.Entity<CarEntry>()
                .Property(x => x.Timestamp)
                .HasColumnName("timestamp")
                .HasColumnType(_typesMapping[nameof(DateTime)]);

            modelBuilder.Entity<CarEntry>()
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType(_typesMapping[nameof(String)]);

            modelBuilder.Entity<DriverEntry>()
                .ToTable("drivers")
                .HasKey(x => x.Timestamp);

            modelBuilder.Entity<DriverEntry>()
                .Property(x => x.Timestamp)
                .HasColumnName("timestamp")
                .HasColumnType(_typesMapping[nameof(DateTime)]);

            modelBuilder.Entity<DriverEntry>()
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType(_typesMapping[nameof(String)]);
        }
    }
}
