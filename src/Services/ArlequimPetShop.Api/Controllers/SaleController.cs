using ArlequimPetShop.Contracts.Commands.Sales;
using ArlequimPetShop.Contracts.Queries.Sales;
using Microsoft.AspNetCore.Mvc;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Api.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de venda, como consulta e criação de vendas.
    /// </summary>
    [ApiController]
    [Route("sales")]
    public class SaleController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        /// <summary>
        /// Construtor do <see cref="SaleController"/> com injeção dos barramentos de comando e requisição.
        /// </summary>
        /// <param name="commandBus">Barramento de comandos para manipulação de vendas.</param>
        /// <param name="requestBus">Barramento de requisições para consultas de vendas.</param>
        public SaleController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        /// <summary>
        /// Consulta todas as vendas com base em filtros definidos no <see cref="SaleQuery"/>.
        /// </summary>
        /// <param name="query">Filtros da consulta.</param>
        /// <returns>Resultado da consulta contendo as vendas encontradas.</returns>
        [HttpGet]
        public async Task<SaleQueryResult> Get([FromQuery] SaleQuery query)
        {
            return await _requestBus.RequestAsync<SaleQuery, SaleQueryResult>(query);
        }

        /// <summary>
        /// Obtém os detalhes de uma venda específica a partir do seu identificador.
        /// </summary>
        /// <param name="id">Identificador da venda.</param>
        /// <returns>Detalhes da venda.</returns>
        [HttpGet("{id}")]
        public async Task<SaleByIdQueryResult> GetDetail(Guid id)
        {
            return await _requestBus.RequestAsync<SaleByIdQuery, SaleByIdQueryResult>(new SaleByIdQuery(id));
        }

        /// <summary>
        /// Cria uma nova venda com os dados enviados no corpo da requisição.
        /// </summary>
        /// <param name="command">Dados da venda.</param>
        /// <returns>Identificador da venda criada.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleCreateCommand command)
        {
            command.Id = Guid.NewGuid();

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }
    }
}