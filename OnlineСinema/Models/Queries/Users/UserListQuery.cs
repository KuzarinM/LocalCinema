using AdstractHelpers.Mediator.Interfaces;
using AdstractHelpers.Storage.Abstraction.Models;
using OnlineСinema.Models.Dtos.Users;

namespace OnlineСinema.Models.Queries.Users
{
    public class UserListQuery: IRequestModel<PaginationModel<UserViewDto>>
    {
        public int PageNumber { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public string? Search {  get; set; }
    }
}
