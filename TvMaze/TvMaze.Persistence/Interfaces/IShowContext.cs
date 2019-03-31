using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMaze.Persistence.Models;

namespace TvMaze.Persistence.Interfaces
{
    /// <summary>
    /// Interface for DbContext for IShowRepository testing.
    /// </summary>
    public interface IShowContext
    {
        /// <summary>
        /// Gets the People who are casted in a Show.
        /// </summary>
        DbSet<Person> People { get; set; }

        /// <summary>
        /// Gets the Shows.
        /// </summary>
        DbSet<Show> Shows { get; }

        /// <summary>
        /// Gets the Casts.
        /// </summary>
        DbSet<Cast> Casts { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
