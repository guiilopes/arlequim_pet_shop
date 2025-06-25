using SrShut.Cqrs.Commands;

namespace ArlequimPetShop.Contracts.Commands.Users
{
    public class UserCreateCommand : ICommand
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password{ get; set; }
    }
}