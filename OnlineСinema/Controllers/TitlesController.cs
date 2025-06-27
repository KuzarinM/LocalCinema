using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models.Queries.Titles;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpGet("full")]
        public Task<IActionResult> GetFullList(
            [FromQuery]
            string? search,
            [FromQuery]
            bool? isFilm,
            [FromQuery]
            List<string>? tags,
            [FromQuery]
            int pageNumber = 0,
            [FromQuery]
            int pageSize = 10,
            CancellationToken cancellationToken = default) =>
            MediatorSendRequest(new TitlesFullListQuery()
            {
                IsFilm = isFilm,
                Search = search,
                Tags = tags,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Principal = HttpContext.User
            },
            cancellationToken,
            null);

        [HttpGet()]
        public Task<IActionResult> GetList(
           [FromQuery]
            string? search,
           [FromQuery]
            bool? isFilm,
           [FromQuery]
            List<string>? tags,
           [FromQuery]
            int pageNumber = 0,
           [FromQuery]
            int pageSize = 10,
           CancellationToken cancellationToken = default) =>
           MediatorSendRequest(new TitlesListQuery()
           {
               IsFilm = isFilm,
               Search = search,
               Tags = tags,
               PageNumber = pageNumber,
               PageSize = pageSize,
               Principal = HttpContext.User
           },
           cancellationToken,
           null);

        [HttpGet("{id:guid}")]
        public Task<IActionResult> GetTitle([FromRoute] Guid id, CancellationToken cancellationToken)
            => MediatorSendRequest(new TitleFullByIdQuery()
            {
                Id = id,
                Principal = HttpContext.User
            },
            cancellationToken,
           null);
    }
}
