using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Titles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class TitleVideoInformationQueryHandler(
        ILogger<TitleVideoInformationQueryHandler> logger,
        IEpisodeStorage episodeStorage,
        ITitleStorage titleStorage,
        IMapper mapper
        ) : AbstractQueryHandler<TitleVideoInformationQuery, TitleVideoInformaionDto>(logger)
    {
        private readonly IEpisodeStorage _episodeStorage = episodeStorage;
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly IMapper _mapper = mapper;

        public async override Task<ResponseModel<TitleVideoInformaionDto>> HandleAsync(TitleVideoInformationQuery request, CancellationToken cancellationToken)
        {
            var identity = request?.Principal.Identity as ClaimsIdentity;

            var userId = Guid.TryParse(
                identity?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var tmp)
                ? tmp
                : (Guid?)null;


            var episode = await _episodeStorage.GetEpisodeWithTitleAndSeen(request!.Id, userId);

            if(episode != null)
            {
                await _episodeStorage.UpdateIsSceen(episode, userId, true);

                return Success(_mapper.Map<TitleVideoInformaionDto>(episode));
            }

            var title = await _titleStorage.GetTitleById(request.Id, userId);

            if (title != null) 
            {
                await _titleStorage.UpdateIsSceen(title, userId, true);

                return Success(_mapper.Map<TitleVideoInformaionDto>(title));
            }

            return Error("Не удалось отправть");
        }
    }
}
