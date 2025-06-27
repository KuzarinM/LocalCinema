using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Models.Internal.Synchonisation;
using OnlineСinema.Models.Queries.Synchronisation;
using System.Text.RegularExpressions;

namespace OnlineСinema.Logic.Handlers.Queries.Synchronisation
{
    public class LoadMediasFromDiskQueryHandler(
        ILogger<LoadMediasFromDiskQueryHandler> logger) : AbstractQueryHandler<LoadMediasFromDiskQuery, List<MediaModel>>(logger)
    {
        public override Task<ResponseModel<List<MediaModel>>> HandleAsync(LoadMediasFromDiskQuery request, CancellationToken cancellationToken)
        {
            List<MediaModel> allMedias = [];
            EnumerateFiles(request.DiskPath, allMedias);

            // Это мы добавляем сами сериалы. С ними всё просто
            var res = allMedias.Where(x => (!x.IsFilm && !x.IsEpisode && !x.IsSeason)).ToList();

            //А вот с фильмами и видеороликами - нет. Они могут быть многосерийными. Их мы приведём в к виду Сериала с 1-м сезоном и сериями внутри

            Dictionary<string, List<MediaModel>> potentioalFilmsParts = new();

            List<MediaModel> potentialSimpleFilms = new();

            // Тут мы делаем простые многосерийные картиты. 
            foreach (var film in allMedias.Where(x => x.IsFilm))
            {
                var filmNameInLower = film.Name.ToLower();

                // Довольно чёткий намёк, что это многосерийник
                if (filmNameInLower.Contains("часть") || filmNameInLower.Contains("эпизод") || Regex.IsMatch(filmNameInLower, @"\d+$"))
                {
                    (string name, string part) = GetPartsFilNameAndNumber(filmNameInLower);

                    film.Name = part;

                    if (potentioalFilmsParts.ContainsKey(name))
                    {
                        potentioalFilmsParts[name].Add(film);
                    }
                    else
                    {
                        potentioalFilmsParts.Add(name, [film]);
                    }
                }
                else
                {
                    potentialSimpleFilms.Add(film);
                }
            }

            potentialSimpleFilms.AddRange(potentioalFilmsParts.Where(x => x.Value.Count == 1).Select(x => x.Value.First()));

            // Возвращаем простым фильмам их нормальные названия
            foreach (var item in potentialSimpleFilms)
            {
                item.Name = Path.GetFileNameWithoutExtension(item.Path);
            }

            res.AddRange(potentialSimpleFilms);

            // Это у нас многосерийные фильмы
            foreach (var item in potentioalFilmsParts.Where(x => x.Value.Count > 1))
            {
                var season = new MediaModel()
                {
                    Name = "tmp",
                    IsSeason = true,
                    Path = Path.GetDirectoryName(item.Value.First().Path)!,
                    Children = item.Value
                };

                res.Add(new()
                {
                    IsFilm = true,
                    Name = item.Key,
                    Path = Path.GetDirectoryName(item.Value.First().Path)!,
                    Children = [season]
                });
            }

            return SuccessTask(res);
        }

        private void EnumerateFiles(string path, List<MediaModel> models)
        {
            foreach (var filePath in Directory.GetFiles(path))
            {

                if(Regex.IsMatch(filePath,@".(mp4)|(avi)|(mkv)$"))
                    AddMedia(filePath, models);
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                try
                {
                    var folderName = Path.GetFileName(directory).ToLower();

                    //То что находится во временной папке - то не надо
                    if (folderName != "tmp" && folderName != "services")
                        EnumerateFiles(directory, models);
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogInformation("Папка {folder} пропущена, из-за отсутствия доступа", directory);
                }

            }
        }
        private void AddMedia(string path, List<MediaModel> medias)
        {
            var filename = Path.GetFileName(path);
            var folderName = Path.GetDirectoryName(path);

            double filmPoints = 0;

            // По идеи у нас всё должно быть в таком виде
            if (Regex.IsMatch(filename.ToLower(), @"(серия)"))
                filmPoints--;
            else
                filmPoints++;

            // Если у нас родительская папка такая вот забавная
            if (Regex.IsMatch(folderName.ToLower(), @"(сезон)|(season)|(S)"))
                filmPoints-=2;
            else
                filmPoints++;

            // Обычно, сырой скаченый сериал нумерует серии или как S1E21 или 1x21, так что это доп проверка
            if(Regex.IsMatch(filename.ToLower(), @"((s)\d+(e)\d+)|(\d+x\d+)"))
                filmPoints-=0.5;

            // Если мы уже что-то из папки признали как эпизод
            if (medias.Any(x => x.Path == folderName && x.IsSeason))
                filmPoints -= 3;

            var parentDir = Path.GetDirectoryName(folderName);

            // Если нет родителя родителя - 100% не сериал
            if (parentDir == null)
                filmPoints+=10;

            if (filmPoints >= 0)
            {
                // Предполагаем, что это фильм

                medias.Add(new()
                {
                    IsFilm = true,
                    Path = path,
                    Name = Path.GetFileNameWithoutExtension(filename).Replace("  "," ").Replace(".",""),
                });
            }
            else
            {
                var series = medias.FirstOrDefault(x => x.Path == parentDir);

                if (series == null)
                {
                    series = new()
                    {
                        Path = parentDir!,
                        Name = Path.GetFileName(parentDir)!,
                    };
                    medias.Add(series);
                }

                var season = medias.FirstOrDefault(x => x.Path == folderName);

                if (season == null)
                {
                    season = new()
                    {
                        Path = folderName,
                        Name = Path.GetFileName(folderName)!,
                        IsSeason = true
                    };

                    medias.Add(season);

                    series.Children.Add(season);
                }

                var episode = new MediaModel()
                {
                    Path = path,
                    Name = filename,
                    IsEpisode = true
                };

                medias.Add(episode);
                season.Children.Add(episode);
            }
        } 

        private (string, string) GetPartsFilNameAndNumber(string filmName)
        {
            int partPosition = filmName.ToLower().IndexOf("часть");
            int episodePosition = filmName.ToLower().IndexOf("эпизод");
            int partNumberPosition = FindStartOfLastNumber(filmName);

            if (partPosition == -1)
                partPosition = 10000;

            if (episodePosition == -1)
                episodePosition = 10000;

            int devider = Math.Min(episodePosition, Math.Min(partPosition, partNumberPosition));

            return (filmName.Substring(0, Math.Max(devider, 0)), filmName.Substring(devider));
        }

        public int FindStartOfLastNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return -1;

            int index = input.Length - 1;

            // Пропускаем нецифровые символы справа
            while (index >= 0 && !char.IsDigit(input[index]))
                index--;

            // Если цифр не найдено
            if (index < 0)
                return -1;

            // Ищем начало числа (первую цифру последовательности)
            while (index >= 0 && char.IsDigit(input[index]))
                index--;

            // Возвращаем индекс первого символа числа
            return index + 1;
        }
    }
}
