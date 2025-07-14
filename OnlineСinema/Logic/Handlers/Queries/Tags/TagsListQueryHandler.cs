using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Tags;
using OnlineСinema.Models.Queries.Tags;

namespace OnlineСinema.Logic.Handlers.Queries.Tags
{
    public class TagsListQueryHandler(
        ILogger<TagsListQueryHandler> logger, 
        ITagStorage tagStorage,
        IMapper mapper
        ) : AbstractQueryHandler<TagsListQuery, PaginationModel<TagDto>>(logger)
    {
        private readonly ITagStorage _tagStorage = tagStorage;
        private readonly IMapper _mapper = mapper;

        public async override Task<ResponseModel<PaginationModel<TagDto>>> HandleAsync(TagsListQuery request, CancellationToken cancellationToken)
        {
            var page = await _tagStorage.GetAllTags(request.PageSize, request.PageNumber);

            return Success(new()
            {
                PageSize = page.PageSize,
                CurrentPage = page.CurrentPage,
                Items = _mapper.Map<List<TagDto>>(page.Items),
                TotalPages = page.TotalPages
            });
        }
    }
}
