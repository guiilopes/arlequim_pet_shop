using ArlequimPetShop.Contracts.Commands.Products;
using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Contracts.Queries.Products;
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
    [Route("products")]
    public class ProductController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        public ProductController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        //[Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet]
        public async Task<ProductQueryResult> Get([FromQuery] ProductQuery query)
        {
            return await _requestBus.RequestAsync<ProductQuery, ProductQueryResult>(query);
        }

        //[Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet("{id}")]
        public async Task<ProductByIdQueryResult> GetDetail(Guid? id, [FromQuery] ProductByIdQuery query)
        {
            return await _requestBus.RequestAsync<ProductByIdQuery, ProductByIdQueryResult>(new ProductByIdQuery(id, query.Name, query.Description));
        }

        //[Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        //[HttpGet("{id}")]
        //public async Task<ProductStockHistoryByIdQueryResult> GetStockHistory([FromQuery] ProductStockHistoryByIdQuery query)
        //{
        //    return await _requestBus.RequestAsync<ProductStockHistoryByIdQuery, ProductStockHistoryByIdQueryResult>(query);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateCommand command)
        {
            command.Id = Guid.NewGuid();

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }

        [HttpPost("stockinventory")]
        public async Task<IActionResult> ImportStock([FromForm] ProductStockInventoryCommand command)
        {
            command.UserName = User.Identity.Name;

            await _commandBus.SendAsync(command);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid? id, [FromForm] ProductUpdateCommand command)
        {
            command.Id = id;

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid? id, [FromForm] ProductDeleteCommand command)
        {
            command.Id = id;

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }
    }
}