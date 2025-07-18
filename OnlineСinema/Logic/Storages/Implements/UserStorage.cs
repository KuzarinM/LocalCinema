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
                EmailConfirmed = true
            };

            var res = await _userManager.CreateAsync(user, createDto.Password);

            if (res.Succeeded)
            {
                try
                {
                    await _userManager.AddToRolesAsync(user, createDto.Roles);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалсь добавить роли");
                }

                return user;
            }

            return null;
        }

        public async Task<List<string>> GetRoles()
            => await _dbContext.Roles.Select(x => x.Name!).ToListAsync();

    }
}
