using AdstractHelpers.Storage.Abstraction;
using AdstractHelpers.Storage.Abstraction.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Users;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class UserStorage(
        ILogger<UserStorage> logger,
        CinemaDbContext dbContext, 
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager
    ): IUserStorage
    {
        private readonly ILogger _logger = logger;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly CinemaDbContext _dbContext = dbContext;

        public Task<IdentityUser?> FindByIdAsync(Guid id)=> _userManager.FindByIdAsync(id.ToString());

        public async Task<PaginationModel<IdentityUser>> GetUsersList(int pageSize = 10, int pageNumber = 0, string? search = "")
        {
            var query = _dbContext.Users
                .Where(x => (string.IsNullOrEmpty(search) || x.Email!.Contains(search)));

            var itemsPaged = await query
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return new()
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = itemsPaged,
                TotalPages = (int)(totalCount * 1f / pageSize + 0.99f),
            };
        }

        public Task<IList<string>> GetUserRoles(IdentityUser user)=> _userManager.GetRolesAsync(user); 

        public async Task<IdentityUser?> AddUser(UserCreateDto createDto)
        {
            var user = new IdentityUser()
            {
                Email = createDto.Email,
                UserName = createDto.Username,
                EmailConfirmed = true
            };

            var res = await _userManager.CreateAsync(user, createDto.Password);

            if (res.Succeeded)
            {
                try
                {
                    await AddRolesIfNeeded(createDto.Roles);

                    await _userManager.AddToRolesAsync(user, createDto.Roles);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалсь добавить роли");
                }

                return user;
            }
            else
            {
                var error = string.Join("\n", res.Errors.Select(x => x.Description));

                throw new ArgumentException(error);
            }
        }

        public async Task<List<string>> GetRoles()
            => await _dbContext.Roles.Select(x => x.Name!).ToListAsync();

        public async Task AddRolesIfNeeded(List<string> roles)
        {
            var exisingRole = await _dbContext.Roles.Where(x => roles.Contains(x.Name!)).Select(x => x.Name!).ToListAsync();

            var notExistingRoles = roles.Where(x=>!exisingRole.Contains(x)).ToList();

            foreach (var item in notExistingRoles)
            {
                await _roleManager.CreateAsync(new()
                {
                    Name = item
                });
            }
        }

        public async Task UpdateUserName(IdentityUser user, string userName)
        {
            if(user.UserName != userName)
            {
                user.UserName = userName;

                await _dbContext.SaveChangesAsync();

                await _userManager.UpdateNormalizedUserNameAsync(user);

            }
        }

        public async Task UpdateEmail(IdentityUser user, string email)
        {
            if (user.Email != email)
            {
                user.Email = email;

                await _dbContext.SaveChangesAsync();

                await _userManager.UpdateNormalizedEmailAsync(user);

            }
        }

        public async Task UpdatePassword(IdentityUser user, string oldPassword, string newPassword)
        {
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task UpdateRoles(IdentityUser user, List<string> targetRoles)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var addingList = targetRoles.Where(x => !roles.Contains(x));

            var removeList = roles.Where(x => !targetRoles.Contains(x));

            if (addingList.Any())
                await _userManager.AddToRolesAsync(user, addingList);

            if (removeList.Any())
                await _userManager.RemoveFromRolesAsync(user, removeList);
        }

        public async Task UpdateEnabled(IdentityUser user, bool newValue)
        {
            await _userManager.SetLockoutEnabledAsync(user, newValue);

            if(newValue)
                await _userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(10));
            else
                await _userManager.SetLockoutEndDateAsync(user, null);
        }
    }
}
