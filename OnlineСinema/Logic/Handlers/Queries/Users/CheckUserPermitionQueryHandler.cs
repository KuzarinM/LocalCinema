using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Models.Enums;
using OnlineСinema.Models.Queries.Users;
using PIHelperSh.Core.Extensions;
using System.Security.Claims;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class CheckUserPermitionQueryHandler(
        ILogger<CheckUserPermitionQueryHandler> logger, 
        UserManager<IdentityUser> userManager
        ) : AbstractQueryHandler<CheckUserPermitionQuery, Dictionary<Permitions, bool>>(logger)
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;


        public override async Task<ResponseModel<Dictionary<Permitions, bool>>> HandleAsync(CheckUserPermitionQuery request, CancellationToken cancellationToken)
        {
            var res = request.RequestedPermitions.Distinct().ToDictionary(x => x, x => false);

            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var tmp)
                    ? tmp
                    : (Guid?)null;

            if (userId == null)
                return Success(res);

            var userModel = await _userManager.FindByIdAsync(userId.ToString()!);

            if(userModel == null) 
                return Success(res);

            bool isAdmin = false;

            // Админу вообще всё можно!
            if(await _userManager.IsInRoleAsync(userModel, "admin"))
                isAdmin = true;

            foreach (var item in res)
            {
                if (isAdmin)
                {
                    res[item.Key] = true;
                    continue;
                }

                foreach (var sepatatedPermitions in Enum.GetValues<Permitions>())
                {
                    if (item.Key.HasFlag(sepatatedPermitions))
                    {
                        if(await _userManager.IsInRoleAsync(userModel, sepatatedPermitions.GetValue<string>()))
                        {
                            res[item.Key] = true;
                        }
                        else
                        {
                            res[item.Key] = false;
                            break;
                        }

                    }
                }
            }

            return Success(res);
        }
    }
}
