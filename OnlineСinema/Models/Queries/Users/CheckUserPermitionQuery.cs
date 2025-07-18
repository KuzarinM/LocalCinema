using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Enums;
using System.Security.Claims;

namespace OnlineСinema.Models.Queries.Users
{
    public class CheckUserPermitionQuery: IRequestModel<Dictionary<Permitions, bool>>
    {
        public List<Permitions> RequestedPermitions { get; set; }

        public ClaimsPrincipal Principal { get; set; }
    }
}
