using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMaze.Domains.DTO;

namespace TvMaze.DataApiClient
{
    public interface ITvMazeService
    {
        Task<ShowDto> GetShowDataById(int showId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ShowDto>> GetShowsByPage(int pageNumber, CancellationToken cancellationToken = default(CancellationToken));
    }
}