using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Titles;
using System.Security.Claims;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class TitleSetIsSceenCommandHandler(
        ILogger<TitleSetIsSceenCommandHandler> logger, 
        ITitleStorage titleStorage,
        IEpisodeStorage episodeStorage
        ) : AbstractCommandHandler<TitleSetIsSceenCommand>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly IEpisodeStorage _episodeStorage = episodeStorage;

        public override async Task<ResponseModel> HandleAsync(TitleSetIsSceenCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.TryParse(
                request.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var tmp)
                    ? tmp
                    : (Guid?)null;

            if (userId == null)
                return Error();

            var episode = await _episodeStorage.GetEpisodeWithSceenById(request.ObjectId, userId);

            if(episode != null)
            {
                await _episodeStorage.UpdateIsSceen(episode, userId, request.SettingValue);

                return Success();
            }

            var title = await _titleStorage.GetTitleById(request.ObjectId, userId);

            if(title != null)
            {
                await _titleStorage.UpdateIsSceen(title, userId, request.SettingValue);

                return Success();
            }

            return Error("Не удалось найти");
        }
    }
}
