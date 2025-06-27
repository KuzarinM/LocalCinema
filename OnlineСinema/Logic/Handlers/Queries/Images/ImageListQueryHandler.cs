using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AdstractHelpers.Storage.Abstraction.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Images;
using OnlineСinema.Models.Queries.Images;

namespace OnlineСinema.Logic.Handlers.Queries.Images
{
    public class ImageListQueryHandler(
        ILogger<ImageListQueryHandler> logger, 
        IImageStorage imageStorage
        ) : AbstractQueryHandler<ImageListQuery, PaginationModel<ImageDto>>(logger)
    {
        private readonly IImageStorage _imageStorage = imageStorage;

        public override async Task<ResponseModel<PaginationModel<ImageDto>>> HandleAsync(ImageListQuery request, CancellationToken cancellationToken)
        {
            return Success(await _imageStorage.GetImages(request.Search, request.PageNumber, request.PageSize));
        }
    }
}
