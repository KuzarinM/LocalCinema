using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Models.Commands.Titles;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Titles;
using System.Text.RegularExpressions;
using tryAGI.OpenAI;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class AutomaticReorderEpisodesAndSeasonesCommandHandler(
        ILogger<AutomaticReorderEpisodesAndSeasonesCommandHandler> logger,
        IMediator mediator) : AbstractCommandHandler<AutomaticReorderEpisodesAndSeasonesCommand>(logger,mediator)
    {
        public override async Task<ResponseModel> HandleAsync(AutomaticReorderEpisodesAndSeasonesCommand request, CancellationToken cancellationToken)
        {

            int totalPages = 1;

            for (int i = 0; i < totalPages; i++)
            {
                var titleList = await MediatorSendAsync(new TitlesFullListQuery()
                {
                    PageNumber = i,
                    PageSize = 10
                });

                if(totalPages != titleList.TotalPages)
                    totalPages = titleList.TotalPages;

                foreach (var item in titleList.Items)
                {
                   await _mediator!.Send(new ChangeOrderInTitleCommand()
                    {
                        Orders = ReorderSeasines(item)
                    },
                    cancellationToken);
                }
            }

            return Success();
        }


        private List<OrderDto> ReorderSeasines(TitleFullDto titleFull)
        {
            var res = new List<OrderDto>();

            var reordered = ReorderByNumber(titleFull.Seasons
                .Where(x=>x.Orderindex == 0)
                .Select(x => (x.Id, x.Name)
                ).ToList());

            foreach (var item in titleFull.Seasons)
            {
                if(item.Orderindex == 0)
                    res.Add(new()
                    {
                        Id = item.Id,
                        OrderIndex = reordered.IndexOf(item.Id)+1
                    });
                res.AddRange(ReorderEpisodes(item));
            }

            return res;
        }

        private List<OrderDto> ReorderEpisodes(SeasonDto season)
        {
            var res = new List<OrderDto>();
            var reordered = ReorderByNumber(season.Episodes
                .Where(x=>x.Orderindex == 0)
                .Select(x => (x.Id, x.Name)).ToList());

            for (int i = 0; i < reordered.Count; i++)
            {
                res.Add(new()
                {
                    Id = reordered[i],
                    OrderIndex = i+1,
                });
            }

            return res;
        }

        private List<Guid> ReorderByNumber(List<(Guid, string)> elements)
        {
            return elements
                .Select(x => (x.Item1, int.TryParse(Regex.Replace(x.Item2, @"[^\d]", ""), out var str) ? str: 0))
                .OrderBy(x => x.Item2)
                .Select(x => x.Item1)
                .ToList();
        } 
    }
}
