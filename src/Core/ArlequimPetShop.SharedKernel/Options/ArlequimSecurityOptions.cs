namespace ArlequimPetShop.SharedKernel.Options
{
    /// <summary>
    /// Representa as opções de segurança utilizadas pelo sistema Arlequim Pet Shop.
    /// </summary>
    public class ArlequimSecurityOptions
    {
        /// <summary>
        /// Chave secreta usada para operações criptográficas, como geração de tokens JWT.
        /// </summary>
        public string? Secret { get; set; }
    }
}