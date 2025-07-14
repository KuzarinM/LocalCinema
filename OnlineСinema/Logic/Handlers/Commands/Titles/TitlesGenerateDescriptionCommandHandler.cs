using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Titles;
using OnlineСinema.Models.Queries.Titles;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class TitlesGenerateDescriptionCommandHandler(
        ILogger<TitlesGenerateDescriptionCommandHandler> logger, 
        IMediator mediator,
        ITitleStorage titleStorage
        ) : AbstractCommandHandler<TitlesGenerateDescriptionCommand>(logger, mediator)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;

        public override async Task<ResponseModel> HandleAsync(TitlesGenerateDescriptionCommand request, CancellationToken cancellationToken)
        {
            var titles = await _titleStorage.GetTitlesWithoutDescription();

            foreach (var title in titles) 
            {
                while (true)
                {
                    try
                    {
                        var description = await MediatorSendAsync(new CreateTitleDescriptionQuery()
                        {
                            TitleId = title.Id,
                        });

                        _logger.LogWarning("{titleName}\n{desctiption}", title.Name, description);

                        await _titleStorage.UpdateDescription(title, description);

                        break;

                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, "Ошибка");

                        await Task.Delay(1000);
                    }
                }
                await Task.Delay(1000);

            }

            return Success();
        }
    }
}
