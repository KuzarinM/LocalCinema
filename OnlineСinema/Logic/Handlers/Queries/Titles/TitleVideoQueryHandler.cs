using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Internal.Titles;
using OnlineСinema.Models.Queries.Titles;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class TitleVideoQueryHandler(
        ILogger<TitleVideoQueryHandler> logger, 
        ITitleStorage titleStorage,
        IEpisodeStorage episodeStorage,
        ITitleCashStorage titleCashStorage,
        IOptions<ConvertationConfiguration> convertationConfig
        ) : AbstractQueryHandler<TitleVideoQuery, TitleVideoModel>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly IEpisodeStorage _episodeStorage = episodeStorage;
        private readonly ITitleCashStorage _titleCashStorage = titleCashStorage;
        private readonly ConvertationConfiguration _convertationConfiguration = convertationConfig.Value;

        public override async Task<ResponseModel<TitleVideoModel>> HandleAsync(TitleVideoQuery request, CancellationToken cancellationToken)
        {
            var cash = _titleCashStorage.GetFilePathFromCash(request.Id);

            if (!string.IsNullOrEmpty(cash))
            {
                return Success(new()
                {
                    Path = cash,
                    ContentType = GetMediaType(cash)
                });
            }

            string? path = null;
            bool isEpisode = false;
            Guid? Id = null;

            var episode = await _episodeStorage.GetItemById(request.Id);

            if(episode != null)
            {
                path = episode.Path;
                isEpisode = true;
                Id = episode.Id;
            }
            else
            {
                var film = await _titleStorage.GetTitleById(request.Id);

                if (film == null)
                {
                    return Error();
                }

                path = film.Path;
                isEpisode = false;
                Id = film.Id;
            }

            if (!File.Exists(path))
                return Error();

            if(Path.GetExtension(path).Replace(".","") != "mp4")
            {
                path = await _titleCashStorage.ConvertToMp4InBackground(
                    request.Id, 
                    path, 
                    _convertationConfiguration.UseGpuAxeliration
                );
            }

            return Success(new TitleVideoModel()
            {
                Path = path,
                ContentType = GetMediaType(path)
            });
        }

        private string GetMediaType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();

            var type = provider.TryGetContentType(path, out var mimeType)
               ? mimeType
               : "application/octet-stream";

            return type;
        }
    }
}
