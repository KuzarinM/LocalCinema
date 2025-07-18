using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Models.Commands.Users;
using tryAGI.OpenAI;

namespace OnlineСinema.Logic.Handlers.Commands.Users
{
    public class RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger, 
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager
        ) : AbstractCommandHandler<RegisterCommand>(logger)
    {

        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        public override async Task<ResponseModel> HandleAsync(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new IdentityUser()
            {
                Email = request.Login,
                UserName = request.Login,
                NormalizedEmail = request.Login,
                NormalizedUserName = request.Login,
                EmailConfirmed = true,
            };

            var res = await _userManager.CreateAsync(user, request.Password);

            if (res.Succeeded) 
            {
                if(await _roleManager.FindByNameAsync("user") == null)
                {
                    await _roleManager.CreateAsync(new()
                    {
                        Name = "user"
                    });
                }

                var roleRes = await _userManager.AddToRoleAsync(user, "User");

                if (!roleRes.Succeeded) {
                    // Если не смогли в роль добавить - то удаляем
                    await _userManager.DeleteAsync(user);

                    return Error($"Не удлалось создать роль для пользователя:\n{string.Join("\n", roleRes.Errors.Select(x => x.Description).ToArray())}");
                }

                return Success();
            }
            else
            {
                return Error($"Не удалось создать пользователя:\n{string.Join("\n", res.Errors.Select(x => x.Description).ToArray())}");
            }            
        }
    }
}
