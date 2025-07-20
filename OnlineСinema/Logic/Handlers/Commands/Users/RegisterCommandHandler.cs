using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Users;
using tryAGI.OpenAI;

namespace OnlineСinema.Logic.Handlers.Commands.Users
{
    public class RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger, 
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserStorage userStorage
        ) : AbstractCommandHandler<RegisterCommand>(logger)
    {

        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IUserStorage _userStorage = userStorage;

        public override async Task<ResponseModel> HandleAsync(RegisterCommand request, CancellationToken cancellationToken)
        {
            request.User.Roles = request.User.Roles.Union(["user"]).Distinct().ToList();

            try
            {
                var res = await _userStorage.AddUser(request.User);
            }
            catch(ArgumentException ex)
            {
                return Error(ex.Message);
            }


            return Success();
        }
    }
}
