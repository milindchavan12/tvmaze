using Microsoft.EntityFrameworkCore;
using TvMaze.Persistence.Interfaces;

namespace TvMaze.Persistence.Models
{
    public class ShowContext : DbContext, IShowContext
    {
        public ShowContext(DbContextOptions<ShowContext> options)
            : base(options)
        { }

        public DbSet<Show> Shows { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Cast> Casts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cast>()
                .HasKey(it => new { it.ShowId, it.CastPersonId });

            modelBuilder.Entity<Cast>()
                .HasOne(it => it.Show)
                .WithMany(it => it.Cast)
                .HasForeignKey(it => it.ShowId);
            modelBuilder.Entity<Cast>()
                .HasOne(it => it.Person)
                .WithMany(it => it.Cast)
                .HasForeignKey(it => it.CastPersonId);

            modelBuilder.Entity<Show>().HasKey("Id");
            modelBuilder.Entity<Show>().Property(s => s.Id).ValueGeneratedNever();

            modelBuilder.Entity<Person>().HasKey("Id");
            modelBuilder.Entity<Person>().Property(s => s.Id).ValueGeneratedNever();
        }
    }
}
