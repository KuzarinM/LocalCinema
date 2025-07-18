using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class UserListQueryHandler(
        ILogger<UserListQueryHandler> logger, 
        IUserStorage userStorage,
        IMapper mapper
        ) : AbstractQueryHandler<UserListQuery, PaginationModel<UserViewDto>>(logger)
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserStorage _userStorage = userStorage;

        public override async Task<ResponseModel<PaginationModel<UserViewDto>>> HandleAsync(UserListQuery request, CancellationToken cancellationToken)
        {
            var res = await _userStorage.GetUsersList(request.PageSize, request.PageNumber, request.Search);

            return Success(new()
            {
                PageSize = res.PageSize,
                CurrentPage = res.CurrentPage,
                TotalPages = res.TotalPages,
                Items = _mapper.Map<List<UserViewDto>>(res.Items)
            });
        }
    }
}
