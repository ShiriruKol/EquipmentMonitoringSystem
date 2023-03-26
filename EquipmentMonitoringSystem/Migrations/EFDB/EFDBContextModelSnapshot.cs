﻿// <auto-generated />
using System;
using EquipmentMonitoringSystem.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EquipmentMonitoringSystem.Migrations.EFDB
{
    [DbContext(typeof(EFDBContext))]
    partial class EFDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FactoryNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Maintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DateMaintenance")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsUnplanned")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("NumberHours")
                        .HasColumnType("double precision");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.ToTable("Maintenances");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.UpcomingMaintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DateMaintenance")
                        .HasColumnType("date");

                    b.Property<string>("EquipmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MaintenanceName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UpcomingMaintenances");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Equipment", b =>
                {
                    b.HasOne("EquipmentMonitoringSystem.DataLayer.Entityes.Group", "Group")
                        .WithMany("Equipments")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Group", b =>
                {
                    b.HasOne("EquipmentMonitoringSystem.DataLayer.Entityes.Station", "Station")
                        .WithMany("Groups")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Maintenance", b =>
                {
                    b.HasOne("EquipmentMonitoringSystem.DataLayer.Entityes.Equipment", "Equipment")
                        .WithMany("Maintenances")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Equipment", b =>
                {
                    b.Navigation("Maintenances");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Group", b =>
                {
                    b.Navigation("Equipments");
                });

            modelBuilder.Entity("EquipmentMonitoringSystem.DataLayer.Entityes.Station", b =>
                {
                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
