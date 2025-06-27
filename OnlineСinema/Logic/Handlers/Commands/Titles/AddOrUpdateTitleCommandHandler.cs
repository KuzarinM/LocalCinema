using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Implements;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models;
using OnlineСinema.Models.Commands.Titles;
using OnlineСinema.Models.Dtos.Titles;
using OnlineСinema.Models.Queries.Images;

namespace OnlineСinema.Logic.Handlers.Commands.Titles
{
    public class AddOrUpdateTitleCommandHandler(
        ILogger<AddOrUpdateTitleCommandHandler> logger, 
        IMapper mapper,
        IMediator mediator,
        ITagStorage tagStorage,
        ITitleStorage titleStorage,
        ISeasonStorage seasonStorage,
        IEpisodeStorage episodeStorage) : AbstractQueryHandler<AddOrUpdateTitleCommand, TitleFullDto>(logger, mediator)
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITagStorage _tagStorage = tagStorage;
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly ISeasonStorage _seasonStorage = seasonStorage;
        private readonly IEpisodeStorage _episodeStorage = episodeStorage;

        public override async Task<ResponseModel<TitleFullDto>> HandleAsync(AddOrUpdateTitleCommand request, CancellationToken cancellationToken)
        {
            var oldTitleModel = await _titleStorage.GetMedia(request.Model.IsFilm, request.Model.Name, request.Model.Path);

            // Нужно добавлять новый 
            if (oldTitleModel == null)
            {
                // Получаем модель
                oldTitleModel = _mapper.Map<Title>(request.Model);

                // Производим обновление тегов
                await AddOrUpdateTags(
                    request.Model.Path.Split('\\'), 
                    oldTitleModel
                );

                await AddOrUpdateImages(oldTitleModel);

                await _titleStorage.AddItem(oldTitleModel);
            }
            else
            {
                await _titleStorage.UpdateNameAndPath(oldTitleModel, request.Model.Name, request.Model.Path);

                // Производим обновление тегов
                await AddOrUpdateTags(
                    request.Model.Path.Split('\\'), 
                    oldTitleModel
                );

                await AddOrUpdateImages(oldTitleModel);

                await _seasonStorage.DeleteExcept(
                    oldTitleModel.Id,
                    request.Model.Children.Select(y => y.Name).ToList(),
                    request.Model.Children.Select(y => y.Path).ToList()
                );

                foreach (var seasone in request.Model.Children)
                {
                    var oldSeason = oldTitleModel.Seasones.FirstOrDefault(x => x.Name == seasone.Name || x.Path == seasone.Path);

                    if(oldSeason == null)
                    {
                        oldSeason = _mapper.Map<Seasone>(seasone);

                        oldSeason.Titleid = oldTitleModel.Id;

                        await _seasonStorage.AddItem(oldSeason);
                    }
                    else
                    {
                        await _seasonStorage.UpdateNameAndPath(oldSeason, seasone.Name, seasone.Path);

                        await _episodeStorage.DeleteExcept(
                            oldSeason.Id,
                            seasone.Children.Select(y => y.Name).ToList(),
                            seasone.Children.Select(y => y.Path).ToList()
                        );

                        foreach (var episode in seasone.Children)
                        {
                            var oldEpisode = oldSeason.Episodes.FirstOrDefault(x => x.Name == episode.Name || x.Path == episode.Path);

                            if(oldEpisode == null)
                            {
                                oldEpisode = _mapper.Map<Episode>(episode);

                                oldEpisode.Seasoneid = oldSeason.Id;

                                await _episodeStorage.AddItem(oldEpisode);
                            }
                            else
                            {
                                await _episodeStorage.UpdateNameAndPath(oldEpisode, episode.Name, episode.Path);
                            }
                        }
                    }
                }
            }

            await _titleStorage.SaveChangesAsync();
            await _tagStorage.SaveChangesAsync();
            await _seasonStorage.SaveChangesAsync();
            await _episodeStorage.SaveChangesAsync();

            return Success(_mapper.Map<TitleFullDto>(oldTitleModel));
        }


        private async Task AddOrUpdateTags(string[] tags, Title title)
        {
            // Первый в пути, точно в муср
            var potentialTags = tags.Skip(1);

            potentialTags = potentialTags.Where(x => 
                x.ToLowerInvariant() != title.Name.ToLowerInvariant() &&
                !x.Contains("."));

            var tagsObjects = await _tagStorage.AddOrUpdateTags(potentialTags.ToArray());

            await _titleStorage.UpdateTagList(title, tagsObjects);
        }

        private async Task AddOrUpdateImages(Title title)
        {
            var tileCover = await MediatorSendAsync(new ImageByParamsQuery()
            {
                IsCover = true,
                Variants = [
                    title.Name
                ]
            });

            var tileTile = await MediatorSendAsync(new ImageByParamsQuery()
            {
                IsCover = false,
                Variants = [
                    title.Name
                ]
            });

            await _titleStorage.UpdateImages(title, tileTile.Id, tileCover.Id);
        }
    }
}
