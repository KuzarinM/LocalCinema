using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Titles;
using System.Security.Claims;

namespace OnlineСinema.Models.Queries.Titles
{
    public class TitleVideoInformationQuery: IRequestModel<TitleVideoInformaionDto>
    {
        public Guid Id { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
