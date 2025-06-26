using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Resultado da consulta de produto por identificador.
    /// </summary>
    public class ProductByIdQueryResult : IRequestResult
    {
        /// <summary>
        /// Inicializa uma nova instância do resultado da consulta.
        /// </summary>
        public ProductByIdQueryResult() { }

        /// <summary>
        /// Identificador único do produto.
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
        /// Preço atual do produto.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Quantidade atual em estoque do produto.
        /// </summary>
        public decimal? StockQuantity { get; set; }

        /// <summary>
        /// Data de vencimento do produto, se aplicável.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Data de criação do produto no sistema.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Data da última atualização do produto.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}