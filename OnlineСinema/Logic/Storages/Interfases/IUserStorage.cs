using AdstractHelpers.Storage.Abstraction.Models;
using Microsoft.AspNetCore.Identity;
using OnlineСinema.Models.Dtos.Users;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface IUserStorage
    {
        public Task<IdentityUser?> FindByIdAsync(Guid id);

        public Task<PaginationModel<IdentityUser>> GetUsersList(int pageSize = 10, int pageNumber = 0, string? search = "");

        public Task<IList<string>> GetUserRoles(IdentityUser user);

        public Task<IdentityUser?> AddUser(UserCreateDto createDto);

        public Task<List<string>> GetRoles();
    }
}
