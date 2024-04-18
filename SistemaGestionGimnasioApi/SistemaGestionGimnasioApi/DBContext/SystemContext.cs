using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using SistemaGestionGimnasioApi.Data.Entities;
using System.Reflection;

namespace SistemaGestionGimnasioApi.DBContext
{
    public class SystemContext : DbContext
    {
        //propiedades
        public SystemContext(DbContextOptions<SystemContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<TrainerActivity> TrainerActivities { get; set; }

        //constructor
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasDiscriminator(u => u.UserType);
            modelBuilder.Entity<TrainerActivity>()
                .HasOne(a => a.Activity)
                .WithMany()
                .HasForeignKey(a => a.IdActivity);
            modelBuilder.Entity<TrainerActivity>()
                .HasOne(t => t.Trainer)
                .WithMany(t=> t.TrainerActivities)
                .HasForeignKey(t => t.TrainerEmail);

            modelBuilder.Entity<GymClass>()
                .HasOne(c => c.Activity)
                .WithMany()
                .HasForeignKey(c => c.IdActivity);

            modelBuilder.Entity<GymClass>()
                .HasOne(c => c.Trainer)
                .WithMany()
                .HasForeignKey(c => c.TrainerEmail);
            modelBuilder.Entity<Reserve>()
                .HasOne(r => r.GymClass)
                .WithMany()
                .HasForeignKey(r => r.IdGymClass);

            modelBuilder.Entity<Reserve>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientEmail);






            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Name = "Juan",
                    LastName = "Perez",
                    Email = "juanperez@gmail.com",
                    Password = "123456",
                    IsDeleted = true,
                    AutorizationToReserve = true
                });
            modelBuilder.Entity<Activity>().HasData(
                new Activity
                {
                    IdActivity = -8,
                    ActivityName = "Fútbol",
                    ActivityDescription = "Actividad deportiva que involucra dos equipos compitiendo por marcar goles en la portería contraria."
                });
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer
                {
                    Name = "Pedro",
                    LastName = "Lopez",
                    Email = "pedrolopez@gmail.com",
                    Password = "123456",
                    IsDeleted = false,
                });
            modelBuilder.Entity<TrainerActivity>().HasData(new TrainerActivity
            {
                IdTrainerActivity = -6,
                IdActivity = -8,
                TrainerEmail ="pedrolopez@gmail.com"
            });
            modelBuilder.Entity<GymClass>().HasData(
                new GymClass
                {
                    IdGymClass = -4,
                    IdActivity= -8,
                    TrainerEmail = "pedrolopez@gmail.com",
                    DateTimeClass = new DateTime(2024, 4, 20, 16, 0, 0),
                    Capacity = 20 
                }
                );

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {

                    Name = "Belen",
                    LastName = "Linera",
                    Email = "belenlinera@gmail.com",
                    Password = "123456",
                    IsDeleted = true
                });
            

        }
        
    }
}
