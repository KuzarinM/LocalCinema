using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AdstractHelpers.Storage.Abstraction.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Titles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class TitlesListQueryHandler(
        ILogger<TitlesListQueryHandler> logger, 
        ITitleStorage titleStorage,
        UserManager<IdentityUser> userManager,
        IOptions<DisplayConfiguration> displayOptions
        ) : AbstractQueryHandler<TitlesListQuery, PaginationModel<TitleDto>>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly DisplayConfiguration _displayConfiguration = displayOptions.Value;

        public override async Task<ResponseModel<PaginationModel<TitleDto>>> HandleAsync(TitlesListQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var tmp)
                    ? tmp
                    : (Guid?)null;

            List<string> forbedenTags = _displayConfiguration.DisallowedTagsList;

            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId!.ToString()!);

                if (user != null)
                {

                    if(await _userManager.IsInRoleAsync(user, "admin"))
                    {
                        forbedenTags.Clear();
                    }
                    else
                    {
                        foreach (var tag in forbedenTags.ToArray())
                        {
                            var allowedRole = $"allow_{tag}";

                            if (await _userManager.IsInRoleAsync(user, allowedRole))
                                forbedenTags.Remove(tag);

                        }
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
