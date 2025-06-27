using AdstractHelpers.Mediator.Interfaces;

namespace OnlineСinema.Models.Commands.Titles
{
    public class SynchronyseTitlesCommand: IRequestModel
    {
        public string Path { get; set; }
    }
}
