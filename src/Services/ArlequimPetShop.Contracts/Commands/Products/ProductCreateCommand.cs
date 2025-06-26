using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    /// <summary>
    /// Comando para criar um novo produto no sistema.
    /// </summary>
    public class ProductCreateCommand : ICommand
    {
        /// <summary>
        /// Identificador único do produto. Gerado internamente.
        /// </summary>
        [JsonIgnore]
        public Guid? Id { get; set; }

        /// <summary>
        /// Código de barras do produto.
        /// </summary>
        [Required]
        public string? Barcode { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Descrição detalhada do produto.
        /// </summary>
        [Required]
        public string? Description { get; set; }

        /// <summary>
        /// Preço unitário do produto.
        /// </summary>
        [Required]
        public decimal? Price { get; set; }

        /// <summary>
        /// Data de validade do produto (opcional).
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
    }
}