using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models.Commands.Images;
using OnlineСinema.Models.Queries.Images;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpPost]
        public Task<IActionResult> UploadImage(string Name, [FromBody] IFormFile file, CancellationToken cancellationToken) => 
            MediatorSendRequest(new UploadImageCommand()
                {
                    Name = Name
                },
                cancellationToken,
                null
            );

        [HttpGet]
        public Task<IActionResult> GetImage(string? search, int pageSize = 10, int pageNumber = 0, CancellationToken cancellationToken = default) =>
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
        public async Task<IActionResult> SyncImage([FromQuery]List<string> texts,  bool isCover, CancellationToken cancellationToken)
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
