using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models.Commands.Images;
using OnlineСinema.Models.Queries.Images;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeOneOfRoles("admin", "edit_galery")]
    public class ImageController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpPost]
        public Task<IActionResult> UploadImage(IFormFile file, string name, bool isCover, CancellationToken cancellationToken)
        {
            MemoryStream stream = new MemoryStream();

            file.CopyTo(stream);

            return MediatorSendRequest(new UploadImageCommand()
                {
                    Name = name,
                    Data = stream,
                    Extention = Path.GetExtension(file.FileName),
                    IsCover = isCover
            },
                cancellationToken,
                null
            );
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(new ImageByIdQuery()
            {
                Id = id
            }, cancellationToken);

            if (res.IsError)
                return StatusCode(res.StatusCode);

                if (res.Data == null)
                return BadRequest();

            return File(res.Data.Data, res.Data.MediaType);
        }

        [HttpDelete("{id:guid}")]
        public Task<IActionResult> DeleteImage([FromRoute] Guid id, CancellationToken cancellationToken)
        => MediatorSendRequest(new DeleteImageByIdCommand()
            {
                Id = id
            }, 
            cancellationToken,
            null);


        [HttpGet]
        public Task<IActionResult> GetImages(string? search, int pageSize = 10, int pageNumber = 0, CancellationToken cancellationToken = default) =>
            MediatorSendRequest(new ImageListQuery()
                {
                    Search = search,
                    PageSize = pageSize,
                    PageNumber = pageNumber
                },
                cancellationToken,
                null
            );

        [HttpGet("closest")]
        public async Task<IActionResult> ClosestImage([FromQuery]List<string> texts,  bool isCover, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(new ImageByParamsQuery()
            {
                IsCover = isCover,
                Variants = texts
            }, cancellationToken);

            if(res.IsError)
                return StatusCode(res.StatusCode);

            return File(res.Data.Data, res.Data.MediaType);
        }
        [HttpPost("sync")]
        public Task<IActionResult> SyncImage(string baseDir, CancellationToken cancellationToken = default) =>
            MediatorSendRequest(new UploadImageFromDiskCommand()
                {
                    DirName = baseDir
                },
                cancellationToken,
                null
            );
    }
}
