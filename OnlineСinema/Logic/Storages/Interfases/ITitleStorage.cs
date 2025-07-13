using AdstractHelpers.Storage.Abstraction.Interfases;
using AdstractHelpers.Storage.Abstraction.Models;
using OnlineСinema.Models;
using OnlineСinema.Models.Dtos.Titles;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface ITitleStorage: IDbStorage<Title, Guid>
    {
        public Task UpdateNameAndPath(Title source, string newName, string newPath);

        public Task<Title?> GetMedia(bool isFilm, string name, string path);

        public Task<Title?> GetTitleById(Guid Id, Guid? userId = null);

        public Task<List<Title>> GetTitlesWithoutDescription();

        public Task<PaginationModel<TitleFullDto>> GetFullTitles(
            Guid? userId,
            string? search, 
            bool? isFilm,
            List<string>? tags,
            int? pageSize = null, 
            int? pageNumber = null
        );

        public Task<PaginationModel<TitleDto>> GetTitles(
            Guid? userId,
            string? search,
            bool? isFilm,
            List<string>? tags,
            int? pageSize = null,
            int? pageNumber = null
        );


        public Task UpdateDescription(Title title, string description);

        public Task UpdateImages(Title title, Guid? tileImageId = null, Guid? coverImageId = null);

        public Task UpdateTagList(Title source, List<Tag> tags);
    }
}
