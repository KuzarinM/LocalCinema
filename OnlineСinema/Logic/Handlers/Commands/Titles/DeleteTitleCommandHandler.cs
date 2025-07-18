
using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Titles;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class DeleteTitleCommandHandler(
        ILogger<DeleteTitleCommandHandler> logger, 
        ITitleStorage titleStorage,
        UserManager<IdentityUser> userManager
        ) : AbstractCommandHandler<DeleteTitleCommand>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async override Task<ResponseModel> HandleAsync(DeleteTitleCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value,
                out var tmp)
                ? tmp
                : (Guid?)null;

            if (userId == null)
                return Error();

            var user = await _userManager.FindByIdAsync(userId.ToString()!);

            if (user == null)
                return Error();

            if(await _userManager.IsInRoleAsync(user, "admin"))
            {

                await _titleStorage.DeleteElement(request.Id);

                return Success();
            }

            return Error();
        }
    }
}
