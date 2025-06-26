using ArlequimPetShop.Contracts.Commands.Users;
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
    /// Controller respons�vel pelas opera��es relacionadas a usu�rios, como cadastro, login e consultas.
    /// </summary>
    [ApiController]
    [Route("users")]
    public class UserController : BaseController
    {
        private readonly ICommandBus _commandBus;
        private readonly IRequestBus _requestBus;

        /// <summary>
        /// Construtor do <see cref="UserController"/> que injeta os barramentos de comando e requisi��o.
        /// </summary>
        /// <param name="commandBus">Barramento de comandos.</param>
        /// <param name="requestBus">Barramento de requisi��es.</param>
        public UserController(ICommandBus commandBus, IRequestBus requestBus) : base()
        {
            Throw.ArgumentIsNull(commandBus);
            Throw.ArgumentIsNull(requestBus);

            _commandBus = commandBus;
            _requestBus = requestBus;
        }

        /// <summary>
        /// Retorna todos os usu�rios com base nos filtros da consulta.
        /// Acesso restrito a administradores e vendedores.
        /// </summary>
        /// <param name="query">Consulta com filtros.</param>
        /// <returns>Resultado da consulta contendo os usu�rios encontrados.</returns>
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Seller}")]
        [HttpGet]
        public async Task<UserQueryResult> Get([FromQuery] UserQuery query)
        {
            return await _requestBus.RequestAsync<UserQuery, UserQueryResult>(query);
        }

        /// <summary>
        /// Cria um novo usu�rio com os dados enviados via formul�rio.
        /// </summary>
        /// <param name="command">Comando com dados do novo usu�rio.</param>
        /// <returns>Identificador do usu�rio criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserCreateCommand command)
        {
            command.Id = Guid.NewGuid();

            await _commandBus.SendAsync(command);

            return Ok(new { command.Id });
        }

        /// <summary>
        /// Realiza login do usu�rio e retorna o token JWT.
        /// </summary>
        /// <param name="command">Comando com e-mail e senha.</param>
        /// <returns>Token JWT em caso de sucesso.</returns>
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] UserLoginCommand command)
        {
            await _commandBus.SendAsync(command);

            return Ok(new { command.Token });
        }
    }
}