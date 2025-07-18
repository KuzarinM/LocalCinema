using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Models.Commands.Titles;
using OnlineСinema.Models.Queries.Images;
using OnlineСinema.Models.Queries.Synchronisation;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class SynchronyseTitlesCommandHandler(
        ILogger<SynchronyseTitlesCommandHandler> logger,
        IMediator mediator,
        CinemaDbContext context) : AbstractCommandHandler<SynchronyseTitlesCommand>(logger, mediator)
    {
        private readonly CinemaDbContext _context = context;
        public override async Task<ResponseModel> HandleAsync(SynchronyseTitlesCommand request, CancellationToken cancellationToken)
        {
            var titles = await MediatorSendAsync(new LoadMediasFromDiskQuery()
            {
                DiskPath = request.Path
            });

            foreach (var title in titles) 
            {

                await MediatorSendAsync(new AddOrUpdateTitleCommand()
                {
                    Model = title,
                });
            }

            // Удалить тайтлы, которых более нет (пока хард делитом)
            //await _context.Titles.Where(x => !titles.Select(x => x.Name).Contains(x.Name)).ExecuteDeleteAsync();

            return Success();

        }
    }
}
