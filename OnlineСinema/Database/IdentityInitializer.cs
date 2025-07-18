using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Models.Enums;
using PIHelperSh.Core.Extensions;

namespace OnlineСinema.Database
{
    public class IdentityInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<DisplayConfiguration> displayConfig
            )
    {

        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly DisplayConfiguration _displayConfiguration = displayConfig.Value;

        private readonly List<string> _systemBaseRoles = [
            "user",
            "admin"
        ];

        private readonly (string login, string password, string role) _defaultUserData = ("admin@admin.com", "admin", "admin");

        public async Task Init()
        {
            await CreateAllRoles();

            await CreateAdmin();
        }

        private async Task CreateAllRoles()
        {
            foreach (var permition in Enum.GetValues<Permitions>())
            {
                _systemBaseRoles.Add(permition.GetValue<string>());
            }

            _systemBaseRoles.Add(_defaultUserData.role);
            
            // Специальные разрешающие теги на запрещёнку
            foreach (var allowedTagPostfix in _displayConfiguration.DisallowedTagsList)
            {
                _systemBaseRoles.Add($"allow_{allowedTagPostfix}");
            }

            foreach (var role in _systemBaseRoles.Distinct())
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task CreateAdmin()
        {
            var user = await _userManager.FindByEmailAsync(_defaultUserData.login);

            if (user == null)
            {
                // Создаём нового админа

                var admin = new IdentityUser()
                {
                    Email = _defaultUserData.login,
                    UserName = "DefaultAdmin"
                };

                var res = await _userManager.CreateAsync(admin, _defaultUserData.password);

                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, _defaultUserData.role);
                }
            }
            else if(!await _userManager.IsInRoleAsync(user, _defaultUserData.role))
            {
                await _userManager.AddToRoleAsync(user, _defaultUserData.role);
            }
        }
    }
}
