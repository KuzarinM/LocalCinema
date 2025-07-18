using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Users;

namespace OnlineСinema.Models.Queries.Users
{
    public class UserByIdQuery: IRequestModel<UserViewDto?>
    {
        public Guid UserId { get; set; }
    }
}
