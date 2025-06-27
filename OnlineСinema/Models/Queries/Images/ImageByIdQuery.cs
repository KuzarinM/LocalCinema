using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Images;

namespace OnlineСinema.Models.Queries.Images
{
    public class ImageByIdQuery: IRequestModel<ImageFullDto>
    {
        public Guid Id { get; set; }
    }
}
