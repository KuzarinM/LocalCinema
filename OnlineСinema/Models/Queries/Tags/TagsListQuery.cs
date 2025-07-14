using AdstractHelpers.Mediator.Interfaces;
using AdstractHelpers.Storage.Abstraction.Models;
using OnlineСinema.Models.Dtos.Tags;
using System.Security.Claims;

namespace OnlineСinema.Models.Queries.Tags
{
    public class TagsListQuery: IRequestModel<PaginationModel<TagDto>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
