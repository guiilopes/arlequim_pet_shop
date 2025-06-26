using SrShut.Cqrs.Commands;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Sales
{
    /// <summary>
    /// Comando responsável por realizar a criação de uma venda.
    /// </summary>
    public class SaleCreateCommand : ICommand
    {
        public SaleCreateCommand()
        {
            Products = new List<SaleProductCommandItem>();
        }

        /// <summary>
        /// Identificador único da venda. Gerado automaticamente no controller e ignorado na serialização JSON.
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Documento do cliente (CPF ou CNPJ) vinculado à venda.
        /// </summary>
        public string? Document { get; set; }

        /// <summary>
        /// Nome do cliente vinculado à venda.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Lista de produtos que fazem parte da venda.
        /// </summary>
        public List<SaleProductCommandItem> Products { get; set; }
    }

    /// <summary>
    /// Item de produto incluído na venda.
    /// </summary>
    public class SaleProductCommandItem
    {
        /// <summary>
        /// Código de barras do produto.
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// Quantidade de unidades do produto vendidas.
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Desconto percentual aplicado ao produto (de 0 a 100).
        /// </summary>
        public decimal? Discount { get; set; }
    }
}