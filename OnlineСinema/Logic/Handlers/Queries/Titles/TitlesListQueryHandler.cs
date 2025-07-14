using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AdstractHelpers.Storage.Abstraction.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Titles;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class TitlesListQueryHandler(
        ILogger<TitlesListQueryHandler> logger, 
        ITitleStorage titleStorage,
        UserManager<IdentityUser> userManager
        ) : AbstractQueryHandler<TitlesListQuery, PaginationModel<TitleDto>>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public override async Task<ResponseModel<PaginationModel<TitleDto>>> HandleAsync(TitlesListQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value,
                out var tmp)
                    ? tmp
                    : (Guid?)null;

            List<string> forbedenTags = ["Ролики", "Вселенная стр", "private"];

            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId!.ToString()!);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("admin"))
                    {
                        forbedenTags.Clear();
                    }
                }
            }

            return Success(await _titleStorage.GetTitles(
                userId,
                request.Search, 
                request.IsFilm, 
                request.Tags, 
                request.PageSize, 
                request.PageNumber,
                forbedenTags)
            );
        }
    }
}
