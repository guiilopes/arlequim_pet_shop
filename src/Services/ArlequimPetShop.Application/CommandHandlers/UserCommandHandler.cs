using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Domain.Users;
using ArlequimPetShop.Domain.Users.Services;
using ArlequimPetShop.SharedKernel.Options;
using Microsoft.Extensions.Options;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Data;

namespace ArlequimPetShop.Application.CommandHandlers
{
    /// <summary>
    /// Manipulador de comandos relacionados ao usuário: criação e login.
    /// </summary>
    public class UserCommandHandler : ICommandHandler<UserCreateCommand>,
                                      ICommandHandler<UserLoginCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkFactory _uofwFactory;
        private readonly string _secret;

        /// <summary>
        /// Construtor do handler com injeção de repositório, unit of work e chave secreta.
        /// </summary>
        public UserCommandHandler(IUserRepository userRepository, IUnitOfWorkFactory uofwFactory, IOptions<ArlequimSecurityOptions> options)
        {
            Throw.ArgumentIsNull(userRepository);
            Throw.ArgumentIsNull(uofwFactory);
            Throw.ArgumentIsNull(options);

            _userRepository = userRepository;
            _uofwFactory = uofwFactory;
            _secret = options.Value.Secret;
        }

        /// <summary>
        /// Manipula o comando de criação de novo usuário.
        /// Verifica duplicidade por e-mail antes de persistir.
        /// </summary>
        /// <param name="command">Comando com dados do usuário a ser criado.</param>
        public async Task HandleAsync(UserCreateCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsTrue(await _userRepository.HasUserByEmail(command.Email), "User.HasUserByEmail", "Usuário já cadastrado com esse email.");

            using var scope = _uofwFactory.Get();

            var user = new User(Guid.NewGuid(), command.Type, command.Name, command.Email, command.Password);

            await _userRepository.AddAsync(user);
            scope.Complete();
        }

        /// <summary>
        /// Manipula o comando de login de usuário.
        /// Valida credenciais e gera token JWT.
        /// </summary>
        /// <param name="command">Comando com e-mail e senha.</param>
        public async Task HandleAsync(UserLoginCommand command)
        {
            Throw.ArgumentIsNull(command);

            using var scope = _uofwFactory.Get();

            Throw.IsFalse(await _userRepository.HasUserByEmail(command.Email), "User.HasUserByEmail", "Usuário não encontrado.");
            Throw.IsFalse(await _userRepository.IsCredentialsValid(command.Email, command.Password), "User.IsCredentialsValid", "Email e/ou senha inválido.");

            var user = await _userRepository.GetAsyncByEmail(command.Email);
            Throw.IsNull(user, "User.GetAsyncByEmail", "Usuário não encontrado");

            command.Token = user.Login(command.Email, _secret);

            scope.Complete();
        }
    }
}