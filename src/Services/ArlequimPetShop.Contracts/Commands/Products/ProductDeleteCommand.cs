using SrShut.Cqrs.Commands;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    /// <summary>
    /// Comando utilizado para excluir um produto do sistema.
    /// </summary>
    public class ProductDeleteCommand : ICommand
    {
        /// <summary>
        /// Identificador único do produto a ser excluído.
        /// Este valor é atribuído internamente via rota da API.
        /// </summary>
        [JsonIgnore]
        public Guid? Id { get; set; }
    }
}