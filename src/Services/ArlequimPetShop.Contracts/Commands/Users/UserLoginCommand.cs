using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Users
{
    /// <summary>
    /// Comando utilizado para autenticar um usuário no sistema.
    /// </summary>
    public class UserLoginCommand : ICommand
    {
        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string? Email { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// Token JWT gerado após autenticação. Ignorado no JSON de entrada.
        /// </summary>
        [JsonIgnore]
        public string? Token { get; set; }
    }
}