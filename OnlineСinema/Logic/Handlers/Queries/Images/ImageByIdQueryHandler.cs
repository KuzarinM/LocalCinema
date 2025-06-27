using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Images;
using OnlineСinema.Models.Queries.Images;

namespace OnlineСinema.Logic.Handlers.Queries.Images
{
    public class ImageByIdQueryHandler(
        ILogger<ImageByIdQueryHandler> logger,
        IImageStorage imageStorage,
        IMapper mapper) : AbstractQueryHandler<ImageByIdQuery, ImageFullDto>(logger)
    {
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;

        public override async Task<ResponseModel<ImageFullDto>> HandleAsync(ImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _imageStorage.GetItemById(request.Id);
            return Success(_mapper.Map<ImageFullDto>(image));
        }
    }
}
