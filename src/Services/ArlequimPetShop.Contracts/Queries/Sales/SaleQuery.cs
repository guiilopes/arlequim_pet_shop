using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    /// <summary>
    /// Query para listagem paginada de vendas com filtro opcional.
    /// </summary>
    public class SaleQuery : PaginationCriteria, IRequest<SaleQueryResult>
    {
        public SaleQuery() { }

        /// <summary>
        /// Construtor que recebe o texto de busca.
        /// </summary>
        /// <param name="text">Texto de busca (cliente, documento ou ID da venda).</param>
        public SaleQuery(string? text) : base()
        {
            Text = text;
        }

        /// <summary>
        /// Texto utilizado para filtro (nome, documento ou ID).
        /// </summary>
        public string? Text { get; set; }
    }
}