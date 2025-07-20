using AdstractHelpers.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineСinema.Logic.Handlers.Queries.Users;
using OnlineСinema.Models.Commands.Users;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Enums;
using OnlineСinema.Models.Queries.Users;
using System.Threading;

namespace OnlineСinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeOneOfRoles("admin", "manage_user")]
    public class UserController(IMediator mediator) : MediatorContoller(mediator)
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LogInQuery logInQuery, CancellationToken cancellationToken) 
            => await MediatorSendRequest(logInQuery, cancellationToken, null);

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto createDto, CancellationToken cancellationToken)
            => await MediatorSendRequest(new RegisterCommand()
            {
                User = createDto
            }, 
            cancellationToken, 
            null);

        [HttpGet("refrash")]
        [AllowAnonymous]
        public async Task<IActionResult> Refrash(string refrashToken, CancellationToken cancellationToken) 
            => await MediatorSendRequest(new RefrashTokensQuery()
            {
                RefrashToken = refrashToken,
            }, 
            cancellationToken,
            null);

        [HttpGet("check")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPermitions([FromQuery] List<Permitions> perm, CancellationToken cancellationToken)
            => await MediatorSendRequest(new CheckUserPermitionQuery()
            {
                RequestedPermitions = perm,
                Principal = HttpContext.User
            },
            cancellationToken,
            null);

        [HttpGet()]
        public Task<IActionResult> GetList(
            CancellationToken cancellationToken,
            [FromQuery] 
            int pageSize = 10, 
            [FromQuery] 
            int pageNumber=0, 
            [FromQuery] 
            string? search = null)
            => MediatorSendRequest(new UserListQuery()
            {
                Search = search,
                PageSize = pageSize,
                PageNumber = pageNumber
            },
            cancellationToken,
            null);

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken cancellationToken)
            => await MediatorSendRequest(new UserByIdQuery()
            {
                UserId = id
            },
            cancellationToken,
            null);

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id, [FromBody] UserUpdateDto updateDto, CancellationToken cancellationToken)
            => await MediatorSendRequest(new UpdateUserQuery()
            {
                Id = id,
                UpdateDto = updateDto,
            },
            cancellationToken,
            null);

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
            => await MediatorSendRequest(new RolesListQuery(),
            cancellationToken,
            null);
    }
}
