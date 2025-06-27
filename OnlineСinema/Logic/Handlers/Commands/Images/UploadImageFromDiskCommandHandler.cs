using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Models.Commands.Images;
using OnlineСinema.Models.Queries.Synchronisation;

namespace OnlineСinema.Logic.Handlers.Commands.Images
{
    public class UploadImageFromDiskCommandHandler(
        ILogger<UploadImageFromDiskCommandHandler> logger, 
        IMediator mediator
        ) : AbstractCommandHandler<UploadImageFromDiskCommand>(logger, mediator)
    {
        public override async Task<ResponseModel> HandleAsync(UploadImageFromDiskCommand request, CancellationToken cancellationToken)
        {
            var images = await MediatorSendAsync(new LoadImagesFromDriveQuery()
            {
                BaseDirPath = request.DirName
            });

            foreach (var image in images) 
            {
                await MediatorSendAsync(new UploadImageCommand()
                {
                    Data = image.Data,
                    Name = image.Name,
                    Extention = Path.GetExtension(image.Path),
                    IsCover = image.Path.ToLower().Contains("фоны")
                });
            }

            return Success();
        }
    }
}
