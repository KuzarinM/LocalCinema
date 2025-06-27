using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Models.Commands.Users;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LogInQuery logInQuery, CancellationToken cancellationToken) 
            => await MediatorSendRequest(logInQuery, cancellationToken, null);

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand registerModel, CancellationToken cancellationToken)
            => await MediatorSendRequest(registerModel, cancellationToken, null);

        [HttpGet("refrash")]
        public async Task<IActionResult> Refrash(string refrashToken, CancellationToken cancellationToken) 
            => await MediatorSendRequest(new RefrashTokensQuery()
            {
                RefrashToken = refrashToken,
            }, 
            cancellationToken,
            null);
    }
}
