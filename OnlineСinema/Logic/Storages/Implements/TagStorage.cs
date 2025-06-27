using AdstractHelpers.Storage.Abstraction;
using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class TagStorage(
        ILogger<TagStorage> logger, 
        IMapper mapper, 
        CinemaDbContext dbContext
        ) : AbstractDbStorage<Tag, Guid>(logger, mapper, dbContext), ITagStorage
    {
        public async Task<List<Tag>> AddOrUpdateTags(string[] tags)
        {
            var exstingTags =  await GetQueryable().Where(x => tags.Contains(x.Name)).ToListAsync();

            var notExistingTags = _mapper.Map<List<Tag>>(tags.Where(x => !exstingTags.Select(x => x.Name).Contains(x)));

            await AddItems(notExistingTags);

            return exstingTags.Union(notExistingTags).ToList();
        }

        public async Task<PaginationModel<Tag>> GetAllTags(int? pageSize = null, int? pageNumber = null)
        {
            if(pageSize != null && pageNumber != null)
            {
                var items = await GetQueryable().Skip(pageSize.Value * pageNumber.Value).Take(pageSize.Value).ToArrayAsync();

                return new PaginationModel<Tag>()
                {
                    PageSize = pageSize.Value,
                    CurrentPage = pageNumber.Value,
                    TotalPages = (int)(GetQueryable().Count() * 1f / pageSize + 0.99f),
                    Items = _mapper.Map<List<Tag>>(items)
                };
            }

            var res = await GetQueryable().ToListAsync();

            return new PaginationModel<Tag>()
            {
                PageSize = res.Count(),
                CurrentPage = 0,
                TotalPages = 1,
                Items = _mapper.Map<List<Tag>>(res)
            };
        }

        protected override Task<Tag> UpdateItem(Tag source, object target)
        {
            throw new NotImplementedException();
        }
    }
}
