using System.ComponentModel;

namespace ArlequimPetShop.SharedKernel.Enums
{
    public enum UserTypes
    {
        [Description("Administrador")]
        ADMINISTRATOR,
        [Description("Vendedor")]
        SELLER,
    }
}