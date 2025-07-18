using AdstractHelpers.Mediator.Interfaces;
using System.Security.Claims;

namespace OnlineСinema.Models.Commands.Titles
{
    public class DeleteTitleCommand: IRequestModel
    {
        public Guid Id { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
