using AdstractHelpers.Mediator.Interfaces;
using System.Security.Claims;

namespace OnlineСinema.Models.Commands.Titles
{
    public class TitleSetIsSceenCommand: IRequestModel
    {
        public Guid ObjectId { get; set; }

        public bool SettingValue { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
