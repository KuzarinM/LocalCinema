using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Users;

namespace OnlineСinema.Models.Queries.Users
{
    public class UpdateUserQuery: IRequestModel<UserViewDto>
    {
        public UserUpdateDto UpdateDto { get; set; }

        public Guid Id { get; set; }
    }
}
