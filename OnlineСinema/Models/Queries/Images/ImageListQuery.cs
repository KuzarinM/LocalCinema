using AdstractHelpers.Mediator.Interfaces;
using AdstractHelpers.Storage.Abstraction.Models;
using OnlineСinema.Models.Dtos.Images;

namespace OnlineСinema.Models.Queries.Images
{
    public class ImageListQuery: IRequestModel<PaginationModel<ImageDto>>
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string? Search {  get; set; }
    }
}
