using System;
using System.Threading;
using System.Threading.Tasks;

namespace TvMaze.DataApiClient
{
    public interface IJsonApiDataReader
    {
        Task<T> GetJsonData<T>(Uri uri, CancellationToken cancellationToken);
    }
}