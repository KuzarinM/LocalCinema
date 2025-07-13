using AdstractHelpers.Controller;
using DEnc;
using DEnc.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                Id = id
            });

            if (res.IsError)
            {
                return StatusCode(res.StatusCode, res.ErrorMessage);
            }

            return PhysicalFile(res.Data.Path, res.Data.ContentType, enableRangeProcessing: true);
        }
    }
}
