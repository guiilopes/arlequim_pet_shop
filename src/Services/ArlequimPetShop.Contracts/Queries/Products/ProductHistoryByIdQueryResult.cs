using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using static ArlequimPetShop.Contracts.Queries.Products.ProductHistoryByIdQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Resultado da consulta do histórico de produtos, com suporte a paginação parcial.
    /// </summary>
    public class ProductHistoryByIdQueryResult : PartialCollection<ProductHistoryByIdQueryResultItem>, IRequestResult
    {
        /// <summary>
        /// Inicializa uma nova instância vazia do resultado da consulta.
        /// </summary>
        public ProductHistoryByIdQueryResult() { }

        /// <summary>
        /// Inicializa o resultado com uma lista de itens.
        /// </summary>
        /// <param name="items">Lista de produtos encontrados.</param>
        public ProductHistoryByIdQueryResult(IList<ProductHistoryByIdQueryResultItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        /// <summary>
        /// Item individual da listagem de produtos.
        /// </summary>
        public class ProductHistoryByIdQueryResultItem
        {
            /// <summary>
            /// Identificador único do histórico.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Identificador único do produto.
            /// </summary>
            public Guid? ProductId { get; set; }

            /// <summary>
            /// Descrição do histórico.
            /// </summary>
            public string? Description { get; set; }

            /// <summary>
            /// Quantidade do produto.
            /// </summary>
            public decimal? Quantity { get; set; }

            /// <summary>
            /// Número da Nota Fiscal.
            /// </summary>
            public string? DocumentFiscalNumber { get; set; }

            /// <summary>
            /// Data de criação do histórico.
            /// </summary>
            public DateTime? CreatedOn { get; set; }

            /// <summary>
            /// Data da última atualização do histórico.
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
    }
}