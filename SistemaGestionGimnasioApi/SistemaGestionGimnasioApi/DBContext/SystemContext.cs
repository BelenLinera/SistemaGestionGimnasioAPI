using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasioApi.Data.Entities;

namespace SistemaGestionGimnasioApi.DBContext
{
    public class SystemContext : DbContext
    {
        //propiedades

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<User>? Users { get; set; }

        //constructor
        public SystemContext(DbContextOptions<SystemContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasDiscriminator(u => u.UserType);
            modelBuilder.Entity<Trainer>()
                .HasOne(a => a.Activity)
                .WithMany()
                .HasForeignKey(a => a.ActivityName);
            modelBuilder.Entity<Class>()
                .HasOne(a => a.Activity)
                .WithMany()
                .HasForeignKey(a => a.ActivityName);
            modelBuilder.Entity<Class>()
                .HasOne(a => a.Trainer)
                .WithMany()
                .HasForeignKey(a => a.TrainerEmail);
            modelBuilder.Entity<Reserve>()
                .HasOne(c => c.Class)
                .WithMany()
                .HasForeignKey(c => c.DateTimeClass);
            modelBuilder.Entity<Reserve>()
                .HasOne(cli => cli.Client)
                .WithMany()
                .HasForeignKey(cli => cli.ClientEmail);






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
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer
                {
                    Name = "Pedro",
                    LastName = "Lopez",
                    Email = "pedrolopez@gmail.com",
                    Password = "123456",
                    IsDeleted = true
                });
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
