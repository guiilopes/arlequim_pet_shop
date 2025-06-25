using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Contracts.Queries.Users;
using ArlequimPetShop.SharedKernel;
using ArlequimPetShop.SharedKernel.Enums;
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

        [Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet]
        public async Task<UserQueryResult> Get([FromQuery] UserQuery query)
        {
            return await _requestBus.RequestAsync<UserQuery, UserQueryResult>(query);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserCreateCommand command)
        {
            command.Id = Guid.NewGuid();

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] UserLoginCommand command)
        {
            await _commandBus.SendAsync(command);

            return Ok(new { command.Token });
        }
    }
}