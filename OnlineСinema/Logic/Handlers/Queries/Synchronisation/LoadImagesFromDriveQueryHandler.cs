using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Models.Internal.Synchonisation;
using OnlineСinema.Models.Queries.Synchronisation;
using System.Text.RegularExpressions;

namespace OnlineСinema.Logic.Handlers.Queries.Synchronisation
{
    public class LoadImagesFromDriveQueryHandler(
        ILogger<LoadImagesFromDriveQueryHandler> logger
        ) : AbstractQueryHandler<LoadImagesFromDriveQuery, List<ImageModel>>(logger)
    {
        public override Task<ResponseModel<List<ImageModel>>> HandleAsync(LoadImagesFromDriveQuery request, CancellationToken cancellationToken)
        {
            List<ImageModel> result = new();

            EnumerateFiles(request.BaseDirPath, result);

            return SuccessTask(result);
        }

        private void EnumerateFiles(string path, List<ImageModel> models)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {

                if (Regex.IsMatch(filePath, @".(png)|(PNG)|(jpg)|(jpeg)$"))
                    models.Add(CreateImageModel(filePath));
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                try
                {
                    var folderName = Path.GetFileName(directory).ToLower();

                    EnumerateFiles(directory, models);
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogInformation("Папка {folder} пропущена, из-за отсутствия доступа", directory);
                }

            }
        }

        private ImageModel CreateImageModel(string path) 
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            MemoryStream stream = new MemoryStream();

            var data = File.ReadAllBytes(path);

            stream.Write(data, 0, data.Length);
            
            stream.Seek(0, SeekOrigin.Begin);

            return new()
            {
                Name = fileName.ToLower(),
                Data = stream,
                Path = path
            };
        }
    }
}
