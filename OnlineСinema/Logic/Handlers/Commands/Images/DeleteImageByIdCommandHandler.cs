using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineСinema.Logic.Storages.Implements;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Commands.Images;

namespace OnlineСinema.Logic.Handlers.Commands.Images
{
    public class DeleteImageByIdCommandHandler(
        ILogger<DeleteImageByIdCommandHandler> logger,
        IImageStorage imageStorage
        ) : AbstractCommandHandler<DeleteImageByIdCommand>(logger)
    {
        private readonly IImageStorage _imageStorage = imageStorage;

        public async override Task<ResponseModel> HandleAsync(DeleteImageByIdCommand request, CancellationToken cancellationToken)
        {
            await _imageStorage.DeleteElement(request.Id);

            return Success();
        }
    }
}
