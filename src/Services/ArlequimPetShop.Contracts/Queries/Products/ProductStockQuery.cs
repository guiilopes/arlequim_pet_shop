using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Consulta paginada de produtos com foco em informações de estoque.
    /// </summary>
    public class ProductStockQuery : PaginationCriteria, IRequest<ProductStockQueryResult>
    {
        /// <summary>
        /// Inicializa uma nova instância da consulta.
        /// </summary>
        public ProductStockQuery() { }

        /// <summary>
        /// Inicializa a consulta com texto de busca.
        /// </summary>
        /// <param name="text">Texto para busca por nome, ID ou descrição.</param>
        public ProductStockQuery(string? text) : base()
        {
            Text = text;
        }

        /// <summary>
        /// Texto de pesquisa para nome, ID ou descrição do produto.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Data inicial do intervalo de vencimento.
        /// </summary>
        public DateTime? StartExpirationDate { get; set; }

        /// <summary>
        /// Data final do intervalo de vencimento.
        /// </summary>
        public DateTime? EndExpirationDate { get; set; }
    }
}