using System.ComponentModel;

namespace ArlequimPetShop.SharedKernel.Enums
{
    /// <summary>
    /// Define os tipos de usuários disponíveis no sistema.
    /// </summary>
    public enum UserTypes
    {
        /// <summary>
        /// Representa um usuário com privilégios administrativos.
        /// </summary>
        [Description("Administrador")]
        ADMINISTRATOR,

        /// <summary>
        /// Representa um usuário do tipo vendedor.
        /// </summary>
        [Description("Vendedor")]
        SELLER,
    }
}