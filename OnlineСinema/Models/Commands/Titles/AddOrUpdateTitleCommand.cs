using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Internal.Synchonisation;

namespace OnlineСinema.Models.Commands.Titles
{
    public class AddOrUpdateTitleCommand: IRequestModel<TitleFullDto>
    {
        public MediaModel Model { get; set; }
    }
}
