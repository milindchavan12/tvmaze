using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMaze.Persistence.Models;

namespace TvMaze.Persistence.Interfaces
{
	/// <summary>
	/// Repository to read information for Shows
	/// </summary>
	public interface IShowRepository
    {
		/// <summary>
		/// Gets the <see cref="Show"/> including the <see cref="Show.Cast"/>.
		/// </summary>
		Task<List<Show>> GetShowsWithCast(int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a <see cref="Show"/> to the repository.
        /// </summary>
        Task AddShow(Show show);

        /// <summary>
        /// Check if the <see cref="Person"/> is present in the repository.
        /// </summary>
        bool HasPersonWithId(int personId);

        /// <summary>
        /// Gets the last <see cref="Show.Id"/> stored in the <see cref="IShowRepository"/> implementation.
        /// </summary>
        /// <returns></returns>
        Task<int> GetLastShowId();
    }
}