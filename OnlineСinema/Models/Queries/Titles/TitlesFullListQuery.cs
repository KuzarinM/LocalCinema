using AdstractHelpers.Mediator.Interfaces;
using AdstractHelpers.Storage.Abstraction.Models;
using MediatR;
using OnlineСinema.Models.Dtos.Titles;
using System.Security.Claims;

namespace OnlineСinema.Models.Queries.Titles
{
    public class TitlesFullListQuery: IRequestModel<PaginationModel<TitleFullDto>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; } 

        public string? Search { get; set; }

        public bool? IsFilm { get; set; }

        public List<string>? Tags { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
