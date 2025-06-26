using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    /// <summary>
    /// Comando para atualização dos dados de um produto no sistema.
    /// </summary>
    public class ProductUpdateCommand : ICommand
    {
        /// <summary>
        /// Identificador único do produto que será atualizado.
        /// Este campo é preenchido automaticamente via rota e ignorado na serialização JSON.
        /// </summary>
        [JsonIgnore]
        public Guid? Id { get; set; }

        /// <summary>
        /// Código de barras do produto.
        /// </summary>
        [Required(ErrorMessage = "O código de barras é obrigatório.")]
        public string? Barcode { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string? Name { get; set; }

        /// <summary>
        /// Descrição do produto.
        /// </summary>
        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public string? Description { get; set; }

        /// <summary>
        /// Preço de venda do produto.
        /// </summary>
        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Data de validade do produto (caso aplicável).
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
    }
}