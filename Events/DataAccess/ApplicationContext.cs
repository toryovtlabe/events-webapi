using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscription>(e =>
            {
                e.HasKey(s => s.Id);
                e.HasOne(s => s.Event).WithMany(e => e.Subscriptions);
                e.HasOne(s => s.User).WithMany(e => e.Subscriptions);
                e.Property(s => s.RegistrationDate).IsRequired();
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.FirstName).IsRequired().HasMaxLength(30);
                e.Property(u => u.Surname).IsRequired().HasMaxLength(40);
                e.Property(u => u.Login).IsRequired().HasMaxLength(30);
                e.HasIndex(u => u.Login).IsUnique();
                e.Property(u => u.BirthDay).IsRequired();
                e.Property(u => u.Password).IsRequired();
                e.Property(u => u.Email).IsRequired().HasMaxLength(255);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.IsAdmin).IsRequired();
                e.HasMany(u => u.Subscriptions).WithOne(s => s.User);
            });

            modelBuilder.Entity<Event>(e =>
            {
                e.HasKey(ev => ev.Id);
                e.Property(ev => ev.Name).IsRequired().HasMaxLength(50);
                e.Property(ev => ev.Description).HasMaxLength(999);
                e.Property(ev => ev.Date).IsRequired();
                e.Property(ev => ev.MaxParticipants).IsRequired().HasMaxLength(5);
                e.HasMany(ev => ev.Subscriptions).WithOne(s => s.Event);
                e.HasMany(ev => ev.Categories).WithMany(c => c.Events);
                e.HasOne(ev => ev.Place).WithMany(p => p.Event);
            });

            modelBuilder.Entity<Place>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).IsRequired().HasMaxLength(50);
                e.HasIndex(p => p.Name).IsUnique();
                e.HasMany(p => p.Event).WithOne(ev => ev.Place);
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(30);
                e.HasIndex(c => c.Name).IsUnique();
                e.HasMany(c => c.Events).WithMany(e => e.Categories);
            });
        }
    }
}
