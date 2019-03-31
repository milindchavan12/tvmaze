using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvMaze.Persistence.Interfaces;
using TvMaze.Persistence.Models;

namespace TvMaze.Persistence.Repositories
{
    /// <summary>
    /// A repository for locally stored shows.
    /// </summary>
    public class ShowRepository : IShowRepository
    {
        private readonly ILogger<ShowRepository> _logger;
        private readonly ShowContext _showContext;

        public ShowRepository(
            ILogger<ShowRepository> logger,
            ShowContext showContext)
        {
            _logger = logger;
            _showContext = showContext;
        }

        public async Task AddShow(Show show)
        {
            if (_showContext.Shows.Any(x => x.Id == show.Id))
            {
                return;
            }

            _logger.LogDebug($"Adding Show {show}");
            await _showContext.Shows.AddAsync(show);
            await _showContext.SaveChangesAsync();
        }

        public bool HasPersonWithId(int personId)
        {
            return _showContext.People.Any(x=> x.Id == personId);
        }

        public async Task<int> GetLastShowId()
	    {
		    return await _showContext.Shows.DefaultIfEmpty().MaxAsync(show => show.Id);
	    }

		public async Task<List<Show>> GetShowsWithCast(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Getting page {pageNumber} with {pageSize} shows with cast");

            var shows = await _showContext.Shows
                .Include(s => s.Cast)
                .ThenInclude(scm => scm.Person)
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return shows.ToList();
        }
    }
}