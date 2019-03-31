using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TvMaze.Persistence.Interfaces;
using TvMaze.Persistence.Models;
using TvMaze.Domains.DTO;

namespace TvMaze.ApiService.Controllers
{
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository _repository;
        private readonly IMapper _mapper;
        public ShowController(IShowRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/Shows")]
        public async Task<ActionResult<IList<ShowDto>>> GetShows([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1)
            {
                return BadRequest($"{nameof(pageNumber)} parameter value should be minimum 1. (Current value: {pageNumber})");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                BadRequest(
                    $"{nameof(pageSize)} parameter value should be between 1 and 100. (Current value: {pageSize})");
            }

            var showList = await _repository.GetShowsWithCast(pageNumber, pageSize, cancellationToken);

            var showDtos = _mapper.Map<List<Show>, List<ShowDto>>(showList);
            showDtos.ForEach(showDto => showDto.Cast = showDto.Cast.OrderByDescending(personDto => personDto.Birthdate).ToList());

            if (showDtos.Count <= 0)
            {
                return BadRequest(
                    $"No Show data found in the repository for {nameof(pageNumber)} {pageNumber}. " +
                    "First run the TvMazeDataIntegrator.Console console application to get Show data from the TvMaze API.");
            }

            return showDtos;
        }
    }
}