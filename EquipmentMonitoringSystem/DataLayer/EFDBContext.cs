using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.EntityFrameworkCore;
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
                entity.Property(x => x.arrayRepair);
                entity.Property(x => x.arrayMouthRepair);
                entity.HasOne(x => x.Group).WithMany(x => x.Equipments).HasForeignKey(x => x.GroupId);
            });
        }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Station> Stations { get; set; }
    }
}
