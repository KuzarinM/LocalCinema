using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Titles;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class UpdateTitleQuery: IRequestModel<TitleDto>
    {
        public Guid Id { get; set; }
        public UpdateTitleDto dto { get; set; }
    }
}
