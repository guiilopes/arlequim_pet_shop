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
    public class UserCommandHandler : ICommandHandler<UserCreateCommand>,
                                      ICommandHandler<UserLoginCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkFactory _uofwFactory;
        private readonly string _secret;

        public UserCommandHandler(IUserRepository userRepository, IUnitOfWorkFactory uofwFactory, IOptions<ArlequimSecurityOptions> options)
        {
            Throw.ArgumentIsNull(userRepository);
            Throw.ArgumentIsNull(uofwFactory);
            Throw.ArgumentIsNull(options);

            _userRepository = userRepository;
            _uofwFactory = uofwFactory;
            _secret = options.Value.Secret;
        }

        public async Task HandleAsync(UserCreateCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsTrue(await _userRepository.HasUserByEmail(command.Email), "User.HasUserByEmail", "Usuário já cadastrado com esse email.");

            using var scope = _uofwFactory.Get();

            var user = new User(Guid.NewGuid(), command.Type, command.Name, command.Email, command.Password);

            await _userRepository.AddAsync(user);

            scope.Complete();
        }

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