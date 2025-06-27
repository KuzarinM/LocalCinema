using AdstractHelpers.Storage.Abstraction.Interfases;
using AdstractHelpers.Storage.Abstraction.Models;
using OnlineСinema.Models;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface ITagStorage: IDbStorage<Tag,Guid>
    {
        public Task<List<Tag>> AddOrUpdateTags(string[] tags);

        public Task<PaginationModel<Tag>> GetAllTags(int? pageSize = null, int? pageNumber = null);
    }
}
