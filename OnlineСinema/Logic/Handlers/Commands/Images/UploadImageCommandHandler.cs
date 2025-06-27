using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Images;

namespace OnlineСinema.Logic.Handlers.Commands.Images
{
    public class UploadImageCommandHandler(
        ILogger<UploadImageCommandHandler> logger, 
        IImageStorage imageStorage
        ) : AbstractQueryHandler<UploadImageCommand, Guid>(logger)
    {
        private readonly IImageStorage _imageStorage = imageStorage;

        public override async Task<ResponseModel<Guid>> HandleAsync(UploadImageCommand request, CancellationToken cancellationToken)
        {
            return Success(await _imageStorage.AddOrUpdateImage(request.Name, request.Data, request.Extention, request.IsCover));
        }
    }
}
