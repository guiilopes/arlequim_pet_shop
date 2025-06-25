using ArlequimPetShop.Contracts.Queries.Users;
using Microsoft.Extensions.Configuration;
using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Application.QueryHandlers
{
    public class UserQueryHandler : BaseDataAccess,
                                   IRequestHandler<UserQuery, UserQueryResult>

    {
        public UserQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<UserQueryResult> HandleAsync(UserQuery query)
        {
            throw new NotImplementedException();
        }
    }
}