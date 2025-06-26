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
    /// <summary>
    /// Controlador responsável pelas operações relacionadas a produtos.
    /// </summary>
    [ApiController]
    [Route("products")]
    public class ProductController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        /// <summary>
        /// Construtor do ProductController que injeta os barramentos de comandos e requisições.
        /// </summary>
        public ProductController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        /// <summary>
        /// Consulta os produtos com base nos filtros do <see cref="ProductQuery"/>.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        public async Task<ProductQueryResult> Get([FromQuery] ProductQuery query)
        {
            return await _requestBus.RequestAsync<ProductQuery, ProductQueryResult>(query);
        }

        /// <summary>
        /// Obtém os detalhes de um produto por ID.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="query">Consulta com filtros adicionais.</param>
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet("{id}")]
        public async Task<ProductByIdQueryResult> GetDetail(Guid? id, [FromQuery] ProductByIdQuery query)
        {
            return await _requestBus.RequestAsync<ProductByIdQuery, ProductByIdQueryResult>(
                new ProductByIdQuery(id, query.Name, query.Description));
        }

        /// <summary>
        /// Consulta os estoques de produtos com base no <see cref="ProductStockQuery"/>.
        /// </summary>
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet("stocks")]
        public async Task<ProductStockQueryResult> GetStocks([FromQuery] ProductStockQuery query)
        {
            return await _requestBus.RequestAsync<ProductStockQuery, ProductStockQueryResult>(query);
        }

        /// <summary>
        /// Cria um novo produto com base nos dados enviados via formulário.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateCommand command)
        {
            command.Id = Guid.NewGuid();

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }

        /// <summary>
        /// Importa inventário de estoque para um ou mais produtos.
        /// </summary>
        [HttpPost("stockinventory")]
        public async Task<IActionResult> ImportStock([FromForm] ProductStockInventoryCommand command)
        {
            command.UserName = User.Identity.Name;

            await _commandBus.SendAsync(command);

            return Ok();
        }

        /// <summary>
        /// Atualiza os dados de um produto existente.
        /// </summary>
        /// <param name="id">Identificador do produto a ser atualizado.</param>
        /// <param name="command">Dados atualizados do produto.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid? id, [FromForm] ProductUpdateCommand command)
        {
            command.Id = id;

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }

        /// <summary>
        /// Exclui um produto com base no ID informado.
        /// </summary>
        /// <param name="id">Identificador do produto a ser excluído.</param>
        /// <param name="command">Comando contendo o ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid? id, [FromForm] ProductDeleteCommand command)
        {
            command.Id = id;

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }
    }
}