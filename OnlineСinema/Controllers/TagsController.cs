using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models;
using OnlineСinema.Models.Queries.Tags;
using OnlineСinema.Models.Queries.Titles;
using System.Threading;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpGet]
        public Task<IActionResult> GetTags(int pageNumber, int pageSize, CancellationToken cancellationToken)
        => MediatorSendRequest(new TagsListQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Principal = HttpContext.User
            },
           cancellationToken,
           null);
    }
}
