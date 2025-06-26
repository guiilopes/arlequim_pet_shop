using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Parâmetros para consulta do histórico de produtos com suporte a paginação e filtros.
    /// </summary>
    public class ProductHistoryByIdQuery : PaginationCriteria, IRequest<ProductHistoryByIdQueryResult>
    {
        /// <summary>
        /// Inicializa uma nova instância da consulta de produtos.
        /// </summary>
        public ProductHistoryByIdQuery() { }

        /// <summary>
        /// Inicializa a consulta com um texto de pesquisa.
        /// </summary>
        /// <param name="id">Texto para busca ID.</param>
        public ProductHistoryByIdQuery(Guid? id) : base()
        {
            Id = id;
        }

        /// <summary>
        /// Identificador do produto.
        /// </summary>
        public Guid? Id { get; set; }
    }
}