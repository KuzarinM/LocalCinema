using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class LogInQueryHandler(
        ILogger<LogInQueryHandler> logger, 
        IMediator mediator,
        UserManager<IdentityUser> userManager,
        IOptions<JwtConfiguration> jwtConfig
        ) : AbstractQueryHandler<LogInQuery, UserLoginDto>(logger, mediator)
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly JwtConfiguration _jwtConfiguration = jwtConfig.Value;

        public override async Task<ResponseModel<UserLoginDto>> HandleAsync(LogInQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Login);

            if (user == null)
                return Error("Пользователя с таким Login не найдено");

            if (!user.LockoutEnabled)
                return Error("Пользоавтель заблокирован");

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var res = await MediatorSendAsync(new GenerateJwtTokenQuery()
                {
                    UserModel = user,
                    Roles = userRoles.ToList()
                });

                return Success(res);
            }

            return Error("Не верный пароль");
        }
    }
}
