namespace ArlequimPetShop.Contracts.Queries.Sales
{
    /// <summary>
    /// Representa os dados de um produto associado a uma venda.
    /// </summary>
    public class SaleProductQueryItem
    {
        /// <summary>
        /// Identificador da venda.
        /// </summary>
        public Guid? SaleId { get; set; }

        /// <summary>
        /// Identificador do produto.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descrição do produto.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Data de validade do produto.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Quantidade vendida.
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Preço unitário do produto.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Porcentagem de desconto aplicada.
        /// </summary>
        public decimal? Discount { get; set; }

        /// <summary>
        /// Preço final com desconto.
        /// </summary>
        public decimal? NetPrice { get; set; }
    }
}