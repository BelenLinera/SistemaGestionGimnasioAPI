using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasioApi.Data.Entities;

namespace SistemaGestionGimnasioApi.Data
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
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    UserId = 1,
                    Name = "Juan",
                    LastName = "Perez",
                    Email = "juanperez@gmail.com",
                    Password = "123456",
                    UserName = "juan.perez",
                    State = true,
                    AutorizationToReserve = true
                });
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer
                {
                    UserId = 2,
                    Name = "Pedro",
                    LastName = "Lopez",
                    Email = "pedrolopez@gmail.com",
                    Password = "123456",
                    UserName = "pedro.lopez",
                    State = true
                });
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    UserId = 3,
                    Name = "Belen",
                    LastName = "Linera",
                    Email = "belenlinera@gmail.com",
                    Password = "123456",
                    UserName = "belen.linera",
                    State = true
                });
            //// Relación 1: Trainer 0..N a Activity 0..M
            //modelBuilder.Entity<Trainer>()
            //    .HasMany(t => t.Activities)
            //    .WithMany(a => a.Trainers)
            //    .Map(m =>
            //    {
            //        m.ToTable("TrainerActivities");
            //        m.MapLeftKey("TrainerId");
            //        m.MapRightKey("ActivityName");
            //    });

            //// Relación 2: Trainer 1 a Class N
            //modelBuilder.Entity<Class>()
            //    .HasRequired(c => c.Trainer)
            //    .WithMany(t => t.Classes)
            //    .HasForeignKey(c => c.TrainerId);

            //// Relación 3: Cliente 1 a Reserve 0..N
            //modelBuilder.Entity<Reserve>()
            //    .HasRequired(r => r.Client)
            //    .WithMany(c => c.Reserves)
            //    .HasForeignKey(r => r.ClientId);

            //// Relación 4: Reserva 0..N a Class 1
            //modelBuilder.Entity<Reserve>()
            //    .HasOptional(r => r.Class)
            //    .WithMany(c => c.Reserves)
            //    .HasForeignKey(r => new { r.ActivityName, r.TrainerId });

            //// Relación 5: Class 0..N a Activity 1
            //modelBuilder.Entity<Class>()
            //    .HasRequired(c => c.Activity)
            //    .WithMany(a => a.Classes)
            //    .HasForeignKey(c => c.ActivityName);
        }
    }
}
