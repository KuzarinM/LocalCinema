using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Dtos.Images;

namespace OnlineСinema.Models.Queries.Images
{
    public class ImageByParamsQuery: IRequestModel<ImageFullDto>
    {
        public bool IsCover { get; set; }

        public List<string> Variants { get; set; }
    }
}
