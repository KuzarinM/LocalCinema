using AdstractHelpers.Mediator.Abstractions;
using AdstractHelpers.Mediator.Models;
using MediatR;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models.Queries.Users;

namespace OnlineСinema.Logic.Handlers.Queries.Users
{
    public class RolesListQueryHandler(
        ILogger<RolesListQueryHandler> logger, 
        IUserStorage userStorage
        ) : AbstractQueryHandler<RolesListQuery, List<string>>(logger)
    {
        private readonly IUserStorage _userStorage = userStorage;

        public override async Task<ResponseModel<List<string>>> HandleAsync(RolesListQuery request, CancellationToken cancellationToken)
        {
            return Success(await _userStorage.GetRoles());
        }
    }
}
