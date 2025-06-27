using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Database;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Queries.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class GenerateJwtTokenQueryHandler(
        ILogger<GenerateJwtTokenQueryHandler> logger,
        IOptions<JwtConfiguration> jwtConfiguration) : AbstractQueryHandler<GenerateJwtTokenQuery, UserLoginDto>(logger)
    {
        private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration.Value;

        public override Task<ResponseModel<UserLoginDto>> HandleAsync(GenerateJwtTokenQuery request, CancellationToken cancellationToken)
        {
            var accessToken = GenerateAccessToken(request.UserModel, request.Roles);
            var refrashToken = GenerateRefrashToken(request.UserModel);

            return SuccessTask(new()
            {
                AccsessTokenLivetime = _jwtConfiguration.LiveTimeMinutes,
                AccessToken = accessToken,
                RefrachToken = refrashToken,
                Login = request.UserModel!.Email!,
                UserRoles = request.Roles
            });
        }

        protected string GenerateRefrashToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            };

            return GenerateTokenCommon(_jwtConfiguration.RefrashTokenSecretKey, claims, 60*24*12);
        }

        protected string GenerateAccessToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return GenerateTokenCommon(_jwtConfiguration.AccessTokenSecretKey, claims, _jwtConfiguration.LiveTimeMinutes);
        }

        protected string GenerateTokenCommon(string secretKey, List<Claim> claims, int tokenLivetime)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(tokenLivetime)),
                Issuer = _jwtConfiguration.TokenIssuer,
                Audience = _jwtConfiguration.TokenAudience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
