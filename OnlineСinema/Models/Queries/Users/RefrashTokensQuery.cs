using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace OnlineСinema.Models.Queries.Users
{
    public class RefrashTokensQuery: IRequestModel<UserLoginDto>
    {
        [Required(ErrorMessage = "Необходимо передать токен")]
        public string RefrashToken { get; set; } = null!;
    }
}
