using AdstractHelpers.Storage.Abstraction;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class SeasonStorage(
        ILogger<SeasonStorage> logger, 
        IMapper mapper, 
        CinemaDbContext dbContext
        ) : AbstractDbStorage<Seasone, Guid>(logger, mapper, dbContext), ISeasonStorage
    {
        public async Task DeleteExcept(Guid titleId, List<string>? names, List<string>? paths)
        {
            names = names ?? [];
            paths = paths ?? [];

            await GetQueryable().Where(x => x.Titleid == titleId && !names.Contains(x.Name) && !paths.Contains(x.Path)).ExecuteDeleteAsync();
        }

        public async Task UpdateNameAndPath(Seasone seasone, string name, string path)
        {
            if (seasone.Name != name)
                seasone.Name = name;

            if (seasone.Path != path)
                seasone.Path = path;

            await _dbContext.SaveChangesAsync();
        }

        protected override Task<Seasone> UpdateItem(Seasone source, object target)
        {
            throw new NotImplementedException();
        }
    }
}
