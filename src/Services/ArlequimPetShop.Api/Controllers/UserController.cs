using ArlequimPetShop.Contracts.Queries.Users;
using ArlequimPetShop.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        public UserController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet]
        public async Task<UserQueryResult> Get(UserQuery query)
        {
            return await _requestBus.RequestAsync<UserQuery, UserQueryResult>(query);
        }
    }
}