﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaGestionGimnasioApi.DBContext;

#nullable disable

namespace SistemaGestionGimnasioApi.Migrations
{
    [DbContext(typeof(SystemContext))]
    partial class SystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Activity", b =>
                {
                    b.Property<int>("IdActivity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ActivityDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdActivity");

                    b.ToTable("Activities");

                    b.HasData(
                        new
                        {
                            IdActivity = -8,
                            ActivityDescription = "Actividad deportiva que involucra dos equipos compitiendo por marcar goles en la portería contraria.",
                            ActivityName = "Fútbol"
                        });
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.GymClass", b =>
                {
                    b.Property<int>("IdGymClass")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeClass")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdActivity")
                        .HasColumnType("int");

                    b.Property<string>("TrainerEmail")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdGymClass");

                    b.HasIndex("IdActivity");

                    b.HasIndex("TrainerEmail");

                    b.ToTable("GymClasses");

                    b.HasData(
                        new
                        {
                            IdGymClass = -4,
                            Capacity = 20,
                            DateTimeClass = new DateTime(2024, 4, 20, 16, 0, 0, 0, DateTimeKind.Unspecified),
                            IdActivity = -6,
                            TrainerEmail = "pedrolopez@gmail.com"
                        });
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Reserve", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClientEmail")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("IdGymClass")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientEmail");

                    b.HasIndex("IdGymClass");

                    b.ToTable("Reserves");
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.TrainerActivity", b =>
                {
                    b.Property<int>("IdTrainerActivity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IdActivity")
                        .HasColumnType("int");

                    b.Property<string>("TrainerEmail")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TrainerEmail1")
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdTrainerActivity");

                    b.HasIndex("IdActivity");

                    b.HasIndex("TrainerEmail");

                    b.HasIndex("TrainerEmail1");

                    b.ToTable("TrainerActivities");

                    b.HasData(
                        new
                        {
                            IdTrainerActivity = -6,
                            IdActivity = -8,
                            TrainerEmail = "pedrolopez@gmail.com"
                        });
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("UserType").HasValue("User");
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Admin", b =>
                {
                    b.HasBaseType("SistemaGestionGimnasioApi.Data.Entities.User");

                    b.HasDiscriminator().HasValue("Admin");

                    b.HasData(
                        new
                        {
                            Email = "belenlinera@gmail.com",
                            IsDeleted = true,
                            LastName = "Linera",
                            Name = "Belen",
                            Password = "123456"
                        });
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Client", b =>
                {
                    b.HasBaseType("SistemaGestionGimnasioApi.Data.Entities.User");

                    b.Property<bool>("AutorizationToReserve")
                        .HasColumnType("tinyint(1)");

                    b.HasDiscriminator().HasValue("Client");

                    b.HasData(
                        new
                        {
                            Email = "juanperez@gmail.com",
                            IsDeleted = true,
                            LastName = "Perez",
                            Name = "Juan",
                            Password = "123456",
                            AutorizationToReserve = true
                        });
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Trainer", b =>
                {
                    b.HasBaseType("SistemaGestionGimnasioApi.Data.Entities.User");

                    b.HasDiscriminator().HasValue("Trainer");

                    b.HasData(
                        new
                        {
                            Email = "pedrolopez@gmail.com",
                            IsDeleted = false,
                            LastName = "Lopez",
                            Name = "Pedro",
                            Password = "123456"
                        });
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.GymClass", b =>
                {
                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("IdActivity")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.Trainer", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Reserve", b =>
                {
                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.GymClass", "GymClass")
                        .WithMany()
                        .HasForeignKey("IdGymClass")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("GymClass");
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.TrainerActivity", b =>
                {
                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("IdActivity")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.Trainer", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SistemaGestionGimnasioApi.Data.Entities.Trainer", null)
                        .WithMany("TrainerActivities")
                        .HasForeignKey("TrainerEmail1");

                    b.Navigation("Activity");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("SistemaGestionGimnasioApi.Data.Entities.Trainer", b =>
                {
                    b.Navigation("TrainerActivities");
                });
#pragma warning restore 612, 618
        }
    }
}