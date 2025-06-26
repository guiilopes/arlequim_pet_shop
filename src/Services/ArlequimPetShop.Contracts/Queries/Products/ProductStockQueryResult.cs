using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using static ArlequimPetShop.Contracts.Queries.Products.ProductStockQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Resultado da consulta paginada de estoque de produtos.
    /// </summary>
    public class ProductStockQueryResult : PartialCollection<ProductStockQueryItem>, IRequestResult
    {
        /// <summary>
        /// Inicializa uma nova instância do resultado da consulta.
        /// </summary>
        public ProductStockQueryResult() { }

        /// <summary>
        /// Inicializa o resultado com uma lista de produtos.
        /// </summary>
        /// <param name="items">Lista de produtos retornados na consulta.</param>
        public ProductStockQueryResult(IList<ProductStockQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        /// <summary>
        /// Item de produto com informações de estoque.
        /// </summary>
        public class ProductStockQueryItem
        {
            /// <summary>
            /// Identificador do produto.
            /// </summary>
            public Guid? ProductId { get; set; }

            /// <summary>
            /// Identificador do registro de estoque.
            /// </summary>
            public int? Id { get; set; }

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
            /// Data de vencimento do produto (se aplicável).
            /// </summary>
            public DateTime? ExpirationDate { get; set; }

            /// <summary>
            /// Quantidade atual disponível em estoque.
            /// </summary>
            public decimal? Quantity { get; set; }

            /// <summary>
            /// Último número de nota fiscal relacionado ao produto.
            /// </summary>
            public string? LastDocumentFiscalNumber { get; set; }

            /// <summary>
            /// Data de criação do registro.
            /// </summary>
            public DateTime? CreatedOn { get; set; }

            /// <summary>
            /// Data da última atualização do registro.
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
    }
}
