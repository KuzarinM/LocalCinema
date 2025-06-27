using AdstractHelpers.Mediator.Interfaces;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Models.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace OnlineСinema.Models.Queries.Users
{
    public class GenerateJwtTokenQuery: IRequestModel<UserLoginDto>
    {
        [Required(ErrorMessage = "Не передана модель пользователя")]
        public IdentityUser UserModel { get; set; } = null!;

        [Required(ErrorMessage = "Не передан список ролей пользователя")]
        public List<string> Roles { get; set; } = null!;
    }
}
