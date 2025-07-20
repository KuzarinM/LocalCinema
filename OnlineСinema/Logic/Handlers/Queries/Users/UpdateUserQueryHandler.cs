using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using AutoMapper;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Dtos.Users;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class UpdateUserQueryHandler(
        ILogger<UpdateUserQueryHandler> logger, 
        IUserStorage userStorage,
        IMapper mapper
        ) : AbstractQueryHandler<UpdateUserQuery, UserViewDto>(logger)
    {
        private readonly IUserStorage _userStorage = userStorage;
        private readonly IMapper _mapper = mapper;

        public override async Task<ResponseModel<UserViewDto>> HandleAsync(UpdateUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userStorage.FindByIdAsync(request.Id);

            if (user == null)
                return Error();

            if (request.UpdateDto.Login != null)
                await _userStorage.UpdateUserName(user, request.UpdateDto.Login);

            if(request.UpdateDto.Email != null)
                await _userStorage.UpdateEmail(user, request.UpdateDto.Email);

            if (request.UpdateDto.NewPassword != null && request.UpdateDto.OldPassword != null)
                await _userStorage.UpdatePassword(user, request.UpdateDto.OldPassword, request.UpdateDto.NewPassword);

            if(request.UpdateDto.Roles != null)
            {
                await _userStorage.AddRolesIfNeeded(request.UpdateDto.Roles);

                await _userStorage.UpdateRoles(user, request.UpdateDto.Roles);
            }

            if (request.UpdateDto.Enabled != null)
                await _userStorage.UpdateEnabled(user, request.UpdateDto.Enabled.Value);

            var res = _mapper.Map<UserViewDto>(user);

            var roles = await _userStorage.GetUserRoles(user);

            res.Roles = roles.ToList();

            return Success(res);
        }
    }
}
