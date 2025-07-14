using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Titles;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class ChangeOrderInTitleCommandHandler(
        ILogger<ChangeOrderInTitleCommandHandler> logger,
        IEpisodeStorage episodeStorage,
        ISeasonStorage seasonStorage
        ) : AbstractCommandHandler<ChangeOrderInTitleCommand>(logger)
    {
        private readonly IEpisodeStorage _episodeStorage = episodeStorage;
        private readonly ISeasonStorage _seasonStorage = seasonStorage;

        public override async Task<ResponseModel> HandleAsync(ChangeOrderInTitleCommand request, CancellationToken cancellationToken)
        {
            var input = request.Orders.Select(x=>(x.Id, x.OrderIndex)).ToArray();

            await _episodeStorage.ChangeOrderIndexesByIds(input);
            await _seasonStorage.ChangeOrderIndexesByIds(input);

            return Success();
        }
    }
}
