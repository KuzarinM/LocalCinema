using AdstractHelpers.Controller;
using DEnc;
using DEnc.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models.Commands.Titles;
using OnlineСinema.Models.Queries.Titles;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVideo([FromRoute] Guid id)
        {

            var res = await _mediator.Send(new TitleVideoQuery()
            {
                Id = id,
                Principal = HttpContext.User
            });

            if (res.IsError)
            {
                return StatusCode(res.StatusCode, res.ErrorMessage);
            }

            return PhysicalFile(res.Data.Path, res.Data.ContentType, enableRangeProcessing: true);
        }

        [HttpGet("info/{id:guid}")]
        public Task<IActionResult> GetVideoInfo([FromRoute] Guid id) => MediatorSendRequest(new TitleVideoInformationQuery()
        {
            Id = id,
            Principal = HttpContext.User
        });

        [HttpPost("{id:guid}/sceen")]
        public Task<IActionResult> SetIsSceen([FromRoute] Guid id, bool isSceen) => MediatorSendRequest(new TitleSetIsSceenCommand()
        {
            ObjectId = id,
            SettingValue = isSceen,
            Principal = HttpContext.User
        });
    }
}
