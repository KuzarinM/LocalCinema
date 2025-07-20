using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace OnlineСinema.Models.Commands.Users
{
    public class RegisterCommand: IRequestModel
    {
        public UserCreateDto User { get; set; }
    }
}
