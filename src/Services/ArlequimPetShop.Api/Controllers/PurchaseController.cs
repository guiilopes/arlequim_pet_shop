using ArlequimPetShop.Contracts.Commands.Products;
using Microsoft.AspNetCore.Mvc;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Api.Controllers
{
    [ApiController]
    [Route("purchases")]
    public class PurchaseController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        public PurchaseController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDocumentFiscalImportCommand command)
        {
            await _commandBus.SendAsync(command);

            return Ok();
        }
    }
}