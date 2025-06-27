using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AdstractHelpers.Storage.Abstraction.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Titles;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class TitlesFullListQueryHandler(
        ILogger<TitlesFullListQueryHandler> logger, 
        ITitleStorage titleStorage
        ) : AbstractQueryHandler<TitlesFullListQuery, PaginationModel<TitleFullDto>>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;

        public override async Task<ResponseModel<PaginationModel<TitleFullDto>>> HandleAsync(TitlesFullListQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value,
                out var tmp) 
                ? tmp 
                : (Guid?) null;

            return Success(await _titleStorage.GetFullTitles(
                userId,
                request.Search, 
                request.IsFilm, 
                request.Tags, 
                request.PageSize, 
                request.PageNumber)
            );
        }
    }
}
