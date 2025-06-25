using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Users
{
    public class UserLoginCommand : ICommand
    {
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [JsonIgnore]
        public string? Token { get; set; }
    }
}