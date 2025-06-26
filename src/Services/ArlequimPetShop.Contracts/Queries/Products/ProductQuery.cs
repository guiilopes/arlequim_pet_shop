using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Parâmetros para consulta de produtos com suporte a paginação e filtros.
    /// </summary>
    public class ProductQuery : PaginationCriteria, IRequest<ProductQueryResult>
    {
        /// <summary>
        /// Inicializa uma nova instância da consulta de produtos.
        /// </summary>
        public ProductQuery() { }

        /// <summary>
        /// Inicializa a consulta com um texto de pesquisa.
        /// </summary>
        /// <param name="text">Texto para busca por nome, descrição ou ID.</param>
        public ProductQuery(string? text) : base()
        {
            Text = text;
        }

        /// <summary>
        /// Texto livre para busca por nome, descrição ou identificador do produto.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Data de início para o filtro por data de vencimento.
        /// </summary>
        public DateTime? StartExpirationDate { get; set; }

        /// <summary>
        /// Data de término para o filtro por data de vencimento.
        /// </summary>
        public DateTime? EndExpirationDate { get; set; }
    }
}