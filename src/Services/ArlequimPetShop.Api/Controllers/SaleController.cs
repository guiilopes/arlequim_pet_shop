using ArlequimPetShop.Contracts.Commands.Sales;
using ArlequimPetShop.Contracts.Queries.Sales;
using Microsoft.AspNetCore.Mvc;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Api.Controllers
{
    [ApiController]
    [Route("sales")]
    public class SaleController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        public SaleController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        //[Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet]
        public async Task<SaleQueryResult> Get([FromQuery] SaleQuery query)
        {
            return await _requestBus.RequestAsync<SaleQuery, SaleQueryResult>(query);
        }

        [HttpGet("{id}")]
        public async Task<SaleByIdQueryResult> GetDetail(Guid id)
        {
            return await _requestBus.RequestAsync<SaleByIdQuery, SaleByIdQueryResult>(new SaleByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleCreateCommand command)
        {
            command.Id = Guid.NewGuid();

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }
    }
}