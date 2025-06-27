using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace OnlineСinema.Models.Queries.Users
{
    public class LogInQuery: IRequestModel<UserLoginDto>
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [EmailAddress]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязатльно")]
        public string Password { get; set; } = null!;
    }
}
