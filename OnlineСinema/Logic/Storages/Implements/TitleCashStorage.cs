using Microsoft.Extensions.Options;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Logic.Storages.Interfases;
using Xabe.FFmpeg;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class TitleCashStorage(
        ILogger<TitleCashStorage> logger,
        IOptions<ConvertationConfiguration> converterConfig): ITitleCashStorage
    {
        private readonly ILogger _logger = logger;
        private readonly ConvertationConfiguration _convertation = converterConfig.Value;

        public async Task<string> ConvertToMp4(Guid id, string path, bool userGpu)
        {
            string output = Path.Combine(_convertation.TepmDirPath, $"{id}.mp4");

            if (!File.Exists(output))
            {
                var mediaInfo = await FFmpeg.GetMediaInfo(path);

                var constructor = FFmpeg.Conversions.New().AddStream(mediaInfo.Streams)
                    .AddParameter("-ac 2")
                    .AddParameter("-movflags +frag_keyframe+empty_moov+default_base_moof")
                    ;

                if (userGpu)
                {
                    constructor = constructor.AddParameter("-c:v h264_nvenc");
                }

                constructor.OnProgress += async (sender, args) =>
                {

                    _logger.LogDebug($"[{args.Duration}/{args.TotalLength}][{args.Percent}%] {id}");
                };

                await constructor
                    .SetOutput(output).SetOutputFormat(Format.mp4).Start();
            }

            return output;
        }

        public async Task<string> ConvertToMp4InBackground(Guid id, string path, bool userGpu)
        {
            _ = Task.Run(() => ConvertToMp4(id, path, userGpu));

            var resPath = Path.Combine(_convertation.TepmDirPath, $"{id}.mp4");

            while (true)
            {
                if(File.Exists(resPath) && (new FileInfo(resPath)).Length > 10)
                {
                    return resPath;
                }

                await Task.Delay(300);
            }
        }

        public string? GetFilePathFromCash(Guid id)
        {
            string output = Path.Combine(_convertation.TepmDirPath, $"{id}.mp4");

            if (File.Exists(output))
                return output;

            return null;
        }

        public async Task<string> GetOrAdd(Guid id, string path, bool userGpu)
        {
            var res = GetFilePathFromCash(id);

            if (string.IsNullOrEmpty(res))
            {
                return await ConvertToMp4(id, path, userGpu);
            }

            return res;
        }
    }
}
