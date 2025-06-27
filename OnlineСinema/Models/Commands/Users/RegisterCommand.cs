using AdstractHelpers.Mediator.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineСinema.Models.Commands.Users
{
    public class RegisterCommand: IRequestModel
    {
        [Required(ErrorMessage = "Логин должен присутствовать")]
        [EmailAddress]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Пароль должен присутстоввать")]
        public string Password { get; set; } = null!;
    }
}
