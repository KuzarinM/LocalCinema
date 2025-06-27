using AdstractHelpers.Mediator.Interfaces;

namespace OnlineСinema.Models.Commands.Images
{
    public class DeleteImageByIdCommand: IRequestModel
    {
        public Guid Id { get; set; }
    }
}
