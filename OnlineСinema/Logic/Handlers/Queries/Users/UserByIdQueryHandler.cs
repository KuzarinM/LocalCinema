using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class UserByIdQueryHandler(
        ILogger<UserByIdQueryHandler> logger, 
        IUserStorage userStorage,
        IMapper mapper
        ) : AbstractQueryHandler<UserByIdQuery, UserViewDto?>(logger)
    {
        private readonly IUserStorage _userStorage = userStorage;
        private readonly IMapper _mapper = mapper;

        public async override Task<ResponseModel<UserViewDto?>> HandleAsync(UserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userStorage.FindByIdAsync(request.UserId);

            if (user == null)
                return Success(null);

            var res = _mapper.Map<UserViewDto>(user);

            var roles = await _userStorage.GetUserRoles(user);

            res.Roles = roles.ToList();

            return Success(res);
        }
    }
}
