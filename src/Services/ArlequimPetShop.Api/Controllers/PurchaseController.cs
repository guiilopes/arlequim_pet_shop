using ArlequimPetShop.Contracts.Commands.Products;
using Microsoft.AspNetCore.Mvc;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Api.Controllers
{
    /// <summary>
    /// Controller respons�vel pelas opera��es de importa��o de compras/faturas fiscais de produtos.
    /// </summary>
    [ApiController]
    [Route("purchases")]
    public class PurchaseController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        /// <summary>
        /// Construtor da controller de compras, com inje��o dos barramentos de comando e requisi��o.
        /// </summary>
        /// <param name="commandBus">Barramento de comandos.</param>
        /// <param name="requestBus">Barramento de requisi��es.</param>
        public PurchaseController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        /// <summary>
        /// Importa um ou mais produtos com base em um documento fiscal enviado via formul�rio.
        /// </summary>
        /// <param name="command">Comando contendo o arquivo XML da nota fiscal e metadados.</param>
        /// <returns>Retorna <see cref="OkResult"/> se importa��o for bem-sucedida.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDocumentFiscalImportCommand command)
        {
            await _commandBus.SendAsync(command);

            return Ok();
        }
    }
}