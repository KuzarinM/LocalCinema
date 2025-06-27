using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Queries.Users;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class RefrashTokensQueryHandler(
        ILogger<RefrashTokensQueryHandler> logger, 
        IMediator mediator,
        IOptions<JwtConfiguration> options,
        UserManager<IdentityUser> userManager) : AbstractQueryHandler<RefrashTokensQuery, UserLoginDto>(logger, mediator)
    {
        private readonly JwtConfiguration _jwtConfiguration = options.Value;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public override async Task<ResponseModel<UserLoginDto>> HandleAsync(RefrashTokensQuery request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(request.RefrashToken);

            if(jsonToken is JwtSecurityToken token)
            {
                try
                {
                    var validationRes = await handler.ValidateTokenAsync(request.RefrashToken, GetValidationParameters());

                    if (!validationRes.IsValid)
                        return Error(message: "Неверный токен", code: 401);
                }
                catch
                {
                    return Error(message: "Неверный токен", code: 401);
                } 

                var userId = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value;

                if (userId == null)
                    return Error(message: "Неверный формат токена");

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return Error("Пользователь не существует");

                var roles = await _userManager.GetRolesAsync(user);

                var res = await MediatorSendAsync(new GenerateJwtTokenQuery()
                {
                    Roles = roles.ToList(),
                    UserModel = user
                });

                return Success(res);
            }

            return Error("Токен не распознан как JWT");
        }

        private TokenValidationParameters GetValidationParameters()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.RefrashTokenSecretKey));

            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,   
                ValidIssuer = _jwtConfiguration.TokenIssuer,
                ValidAudience = _jwtConfiguration.TokenAudience,
                IssuerSigningKey = securityKey,
            };
        }
    }
}
