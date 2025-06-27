using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Implements;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Titles;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class TitleFullByIdQueryHandler(
        ILogger<TitleFullByIdQueryHandler> logger,
        ITitleStorage titleStorage,
        IMapper mapper
        ) : AbstractQueryHandler<TitleFullByIdQuery, TitleFullDto>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly IMapper _mapper = mapper;

        public async override Task<ResponseModel<TitleFullDto>> HandleAsync(TitleFullByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value,
                out var tmp)
                ? tmp
                : (Guid?)null;

            var res = await _titleStorage.GetTitleById(request.Id, userId);

            return Success(_mapper.Map<TitleFullDto>(res));
        }
    }
}
