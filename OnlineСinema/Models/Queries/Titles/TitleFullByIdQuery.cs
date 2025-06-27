using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Titles;
using System.Security.Claims;

namespace OnlineСinema.Models.Queries.Titles
{
    public class TitleFullByIdQuery:IRequestModel<TitleFullDto>
    {
        public Guid Id { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
