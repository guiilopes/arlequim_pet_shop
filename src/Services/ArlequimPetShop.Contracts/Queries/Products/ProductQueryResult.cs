using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using static ArlequimPetShop.Contracts.Queries.Products.ProductQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Resultado da consulta de produtos, com suporte a paginação parcial.
    /// </summary>
    public class ProductQueryResult : PartialCollection<ProductQueryItem>, IRequestResult
    {
        /// <summary>
        /// Inicializa uma nova instância vazia do resultado da consulta.
        /// </summary>
        public ProductQueryResult() { }

        /// <summary>
        /// Inicializa o resultado com uma lista de itens.
        /// </summary>
        /// <param name="items">Lista de produtos encontrados.</param>
        public ProductQueryResult(IList<ProductQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        /// <summary>
        /// Item individual da listagem de produtos.
        /// </summary>
        public class ProductQueryItem
        {
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
            /// Preço unitário do produto.
            /// </summary>
            public decimal? Price { get; set; }

            /// <summary>
            /// Data de vencimento do produto.
            /// </summary>
            public DateTime? ExpirationDate { get; set; }

            /// <summary>
            /// Quantidade disponível em estoque.
            /// </summary>
            public decimal? StockQuantity { get; set; }

            /// <summary>
            /// Data de criação do produto.
            /// </summary>
            public DateTime? CreatedOn { get; set; }

            /// <summary>
            /// Data da última atualização do produto.
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
    }
}