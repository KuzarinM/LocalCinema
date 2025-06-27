using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Images;
using OnlineСinema.Models.Queries.Images;

namespace OnlineСinema.Logic.Handlers.Queries.Images
{
    public class ImageByParamsQueryHandler(
        ILogger<ImageByParamsQueryHandler> logger, 
        IImageStorage imageStorage,
        IMapper mapper
        ) : AbstractQueryHandler<ImageByParamsQuery, ImageFullDto>(logger)
    {
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;

        public override async Task<ResponseModel<ImageFullDto>> HandleAsync(ImageByParamsQuery request, CancellationToken cancellationToken)
        {
            var variants = await _imageStorage.GetImagesVariants(request.Variants.Select(x=>x.ToLower()).ToList(), request.IsCover);

            if(variants.Count() == 0)
            {
                var defualtImage = await _imageStorage.GetImageByName("default");

                return Success(_mapper.Map<ImageFullDto>(defualtImage)); 
            }

            return Success(_mapper.Map<ImageFullDto>(variants.First()));
        }
    }
}
