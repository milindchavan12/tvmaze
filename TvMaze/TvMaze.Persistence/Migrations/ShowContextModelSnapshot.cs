using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using TvMaze.Persistence.Interfaces;

namespace TvMaze.Persistence.Migrations
{
    [DbContext(typeof(IShowContext))]
    public class ShowContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TvMaze.Persistence.Models.Person", b =>
            {
                b.Property<int>("Id");

                b.Property<DateTime?>("Birthdate");

                b.Property<string>("Name");

                b.HasKey("Id");

                b.ToTable("Person");
            });

            modelBuilder.Entity("TvMaze.Persistence.Models.Show", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Name");

                b.HasKey("Id");

                b.ToTable("Shows");
            });

            modelBuilder.Entity("TvMaze.Persistence.Models.Cast", b =>
            {
                b.Property<int>("ShowId");

                b.Property<int>("CastPersonId");

                b.HasKey("ShowId", "CastPersonId");

                b.HasIndex("CastPersonId");

                b.ToTable("Cast");
            });

            modelBuilder.Entity("TvMaze.Persistence.Models.Cast", b =>
            {
                b.HasOne("TvMaze.Persistence.Models.Person", "Person")
                    .WithMany("Cast")
                    .HasForeignKey("CastPersonId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("TvMaze.Persistence.Models.Show", "Show")
                    .WithMany("Cast")
                    .HasForeignKey("ShowId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
