using ArlequimPetShop.SharedKernel.Enums;
using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Users
{
    /// <summary>
    /// Comando para criação de um novo usuário no sistema.
    /// </summary>
    public class UserCreateCommand : ICommand
    {
        /// <summary>
        /// Identificador do usuário (gerado automaticamente). Ignorado no JSON.
        /// </summary>
        [JsonIgnore]
        public Guid? Id { get; set; }

        /// <summary>
        /// Tipo de usuário (Ex: Admin, Seller).
        /// </summary>
        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        public UserTypes Type { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string? Name { get; set; }

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
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string? Password { get; set; }
    }
}