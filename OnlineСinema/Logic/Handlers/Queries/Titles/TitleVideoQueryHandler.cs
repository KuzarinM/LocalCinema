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

            var episode = await _episodeStorage.GetItemById(request.Id);

            TitleVideoModel? res = null;

            if(episode != null)
            {
                res = new() {
                    Path = episode.Path,
                    ContentType = GetMediaType(episode.Path)
                };
            }
            else
            {
                var film = await _titleStorage.GetTitleById(request.Id);

                if (film == null)
                {
                    return Error();
                }

                res = new()
                {
                    Path = film.Path,
                    ContentType = GetMediaType(film.Path)
                };
            }

            if (!File.Exists(res.Path))
                return Error();

            if(Path.GetExtension(res.Path).Replace(".","") != "mp4")
            {
                res.Path = await _titleCashStorage.ConvertToMp4InBackground(request.Id, res.Path, _convertationConfiguration.UseGpuAxeliration);
                res.ContentType = GetMediaType(res.Path);
            }

            return Success(res);
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
