using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Titles;

namespace OnlineСinema.Logic.Handlers.Queries.Titles
{
    public class UpdateTitleQueryHandler(
        ILogger<UpdateTitleQueryHandler> logger, 
        ITitleStorage titleStorage,
        ITagStorage tagStorage,
        IMapper mapper
        ) : AbstractQueryHandler<UpdateTitleQuery, TitleDto>(logger)
    {
        private readonly ITitleStorage _titleStorage = titleStorage;
        private readonly ITagStorage _tagStorage = tagStorage;
        private readonly IMapper _mapper = mapper;

        public async override Task<ResponseModel<TitleDto>> HandleAsync(UpdateTitleQuery request, CancellationToken cancellationToken)
        {
            var model = await _titleStorage.GetItemById(request.Id);

            if (model == null)
                return Error("Нет такого произвдения");

            if (!string.IsNullOrEmpty(request.dto.Description))
                await _titleStorage.UpdateDescription(model, request.dto.Description);

            if (!string.IsNullOrEmpty(request.dto.Name))
                await _titleStorage.UpdateNameAndPath(model, request.dto.Name, model.Path);

            if (request.dto.Coverimageid != null || request.dto.Tileimageid != null)
                await _titleStorage.UpdateImages(model, request.dto.Tileimageid, request.dto.Coverimageid);

            if (request.dto.Isfilm != null)
                await _titleStorage.UpdateIsFilm(model, request.dto.Isfilm.Value);

            if(request.dto.Tags != null)
            {
                var newTags = await _tagStorage.AddOrUpdateTags(request.dto.Tags.ToArray());

                await _tagStorage.SaveChangesAsync();

                await _titleStorage.UpdateTagList(model, newTags);
            }

            await _titleStorage.SaveChangesAsync();

            var res = _mapper.Map<TitleDto>(model);

            return Success(res);
        }
    }
}
