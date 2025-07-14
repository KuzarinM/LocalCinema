using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Titles;

namespace OnlineСinema.Models.Commands.Titles
{
    public class ChangeOrderInTitleCommand: IRequestModel
    {
        public List<OrderDto> Orders { get; set; } = [];
    }
}
