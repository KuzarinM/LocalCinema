using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models.Commands.Titles;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpPost("sync")]
        public async Task<IActionResult> SyncTitles(string diskPath, CancellationToken cancellationToken)
            => await MediatorSendRequest(new SynchronyseTitlesCommand()
            {
                Path = diskPath,
            }, 
            cancellationToken, 
            null);

        [HttpPost("description/generate")]
        public async Task<IActionResult> GenerateDescription(CancellationToken cancellationToken)
            => await MediatorSendRequest(new TitlesGenerateDescriptionCommand()
            {
            },
            cancellationToken,
            null);
    }
}
