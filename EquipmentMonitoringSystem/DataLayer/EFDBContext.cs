using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EquipmentMonitoringSystem.DataLayer
{
    public class EFDBContext : DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) :
            base(options)
        {
            if (Database.GetPendingMigrations().Any())
                Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.Property(x => x.Checkst);
                entity.HasMany(x => x.Groups).WithOne(e => e.Station).HasForeignKey(e => e.StationId);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.Property(x => x.Description);
                entity.HasMany(x => x.Equipments).WithOne(e => e.Group).HasForeignKey(e => e.GroupId);
                entity.HasOne(x => x.Station).WithMany(x => x.Groups).HasForeignKey(x => x.StationId);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.Property(x => x.Type);
                entity.Property(x => x.FactoryNumber);
                entity.HasMany(x => x.Maintenances).WithOne(e => e.Equipment).HasForeignKey(e => e.EquipmentId);
                entity.HasOne(x => x.Group).WithMany(x => x.Equipments).HasForeignKey(x => x.GroupId);
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.Property(x => x.Status);
                entity.Property(x => x.NumberHours);
                entity.Property(x => x.DateMaintenance);
                entity.Property(x => x.IsUnplanned);
                entity.Property(x => x.Description);
                entity.HasOne(x => x.Equipment).WithMany(x => x.Maintenances).HasForeignKey(x => x.EquipmentId);
                entity.HasOne(x => x.UpcomingMaintenance).WithOne(p => p.Maintenance).HasForeignKey<UpcomingMaintenance>(p => p.MaintenancesID);
                entity.HasOne(x => x.Nortify).WithOne(p => p.Maintenance).HasForeignKey<Nortify>(p => p.MaintenancesID);
            });

            modelBuilder.Entity<UpcomingMaintenance>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseSerialColumn();
                entity.Property(x => x.Id).HasValueGenerator<GuidValueGenerator>(); ;
                entity.Property(x => x.MaintenancesID);
            });

            modelBuilder.Entity<Nortify>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Heding);
                entity.Property(x => x.Description);
                entity.Property(x => x.MaintenancesID);
            });
        }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<UpcomingMaintenance> UpcomingMaintenances { get; set; }
        public DbSet<Nortify> Nortifys { get; set;}
    }
}
