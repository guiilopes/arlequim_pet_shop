using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Domain.Users;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Data;

namespace ArlequimPetShop.Application.CommandHandlers
{
    public class UserCommandHandler : ICommandHandler<UserCreateCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWorkFactory _uofwFactory;

        public UserCommandHandler(IUserRepository userRepository, IUnitOfWorkFactory uofwFactory)
        {
            Throw.ArgumentIsNull(userRepository);
            Throw.ArgumentIsNull(uofwFactory);

            _userRepository = userRepository;
            _uofwFactory = uofwFactory;
        }

        public async Task HandleAsync(UserCreateCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsNull(await _userRepository.HasUserByEmail(command.Email), "User.Found", "Usuário já cadastrado com esse email.");

            using var scope = _uofwFactory.Get();

            var user = new User(Guid.NewGuid(), command.Name, command.Email, command.Password);

            await _userRepository.AddAsync(user);

            scope.Complete();
        }
    }
}